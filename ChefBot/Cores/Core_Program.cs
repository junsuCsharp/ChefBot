namespace PackML
{
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Threading;

    public class ClsPackML
    {
        //=========================================================================
        // 개정이력
        //=========================================================================
        // 2021.10.14
        // 정명재
        // ClsProcessor
        //=========================================================================

        //=========================================================================
        // 수정이력 [Index ::: jace]
        //=========================================================================
        // 2022.01.27
        // 정명재
        // namespace 및 class 이름 수정
        // 변수 및 소스 정리
        //=========================================================================

        /*
         * 1. 2022.11.10 교촌 치킨 내부 프로젝트
         * 
         * 
         */

        #region Variable
        const int iCurrentState_MaxCount = 18;

        private int[] iArray_CurrentStateModeMatrix = new int[iCurrentState_MaxCount];  //현재상태모드 2차원 matrix
        private int[] iArray_StateCompleteModeMatrix = new int[iCurrentState_MaxCount]; //현재 상태완료 후 다음 상태 실행 정의를 위한 배열 

        //[Index ::: jace]
        //public int iCurrentStateMode = (int)EModeMatrix.ManualMode_Matrix;            //실행 중인 모드
        //public int iPreviousCurrentStateMode = 0;                                     //이전 실행 중인 모드
        public EModeMatrix eCurrModeMatrix = EModeMatrix.ManualMode_Matrix;
        EModeMatrix ePrevModeMatrix = EModeMatrix.None_Matrix;

        //public int iCurrentState = (int)ECurrState.CurrentState_Stopped;              //실행 중인 상태
        //public int iPreviousState = 0;                                                //실행 중인 상태
        public ECurrState eCurrState = ECurrState.CurrentState_Stopped;
        ECurrState ePrevState = ECurrState.CurrentState_None;

        //public int iExternOld_StateCmmand = (int)ECurrState.CurrentState_Stopped;     //이전 상태 명령
        //public int iExtern_StateCmmand = (int)ECurrState.CurrentState_Stopped;        //외부 상태 명령
        Command eCurrCommand = Command.CurrentState_None;
        Command ePrevCommand = Command.CurrentState_None;

        //private int iArray_CF_Function_State_Number = 0;                         //실행 프로그램 진행 상태        
        //private int iArray_CF_Function_Return_Flag = 0;                          //실행 프로그램 리턴 상태        
        //private int iArray_CF_Function_Start_Flag = 0;                           //실행 프로그램 시작 플래그       

        EProcStep eCF_Function_Run_Flag = EProcStep.CF_Function_Run_Flag_IDLE;   //실행 프로그램 완료 상태 

        EReturn eCommandResult = EReturn.StateCommand_OK;          //명령 상태 결과

        public Stopwatch swProcessTime = new Stopwatch();
        public string strCF_Function_Run_Time = null;                             //실행 시간

        private readonly Dictionary<Command, EStateCommand> dicCommand = new Dictionary<Command, EStateCommand>();

        public delegate void EventHandler(ECurrState state, EModeMatrix mode);
        public event EventHandler Contiused_Action;//for step contiuse
        public event EventHandler CurrentState_Action;//for Step Module
        public event EventHandler AckNormal_Action;//GUI for Action

        DateTime processTime = DateTime.Now;

        //2023.04.25 ::: 초기 인스턴스 전체 생성시 발생 되게끔.
        public bool IsIntanceAction = false;

        public ManualResetEvent mResetEvent = new ManualResetEvent(false);
        #endregion

        public ClsPackML()
        {
            dicCommand.Add(Command.CurrentState_Aborting, EStateCommand.StateCommand_ABORT);
            dicCommand.Add(Command.CurrentState_Clearing, EStateCommand.StateCommand_CLEAR);
            dicCommand.Add(Command.CurrentState_Completing, EStateCommand.StateCommand_COMPLETE);
            dicCommand.Add(Command.CurrentState_Holding, EStateCommand.StateCommand_HOLD);
            dicCommand.Add(Command.CurrentState_None, EStateCommand.StateCommand_NONE);
            dicCommand.Add(Command.CurrentState_Resetting, EStateCommand.StateCommand_RESET);
            dicCommand.Add(Command.CurrentState_Starting, EStateCommand.StateCommand_START);
            dicCommand.Add(Command.CurrentState_Stopping, EStateCommand.StateCommand_STOP);
            dicCommand.Add(Command.CurrentState_Suspended, EStateCommand.StateCommand_UNSUSPEND);
            dicCommand.Add(Command.CurrentState_Suspending, EStateCommand.StateCommand_SUSPEND);
            dicCommand.Add(Command.CurrentState_Unholding, EStateCommand.StateCommand_UNHOLD);

            Thread threadProcessor = new Thread(new ThreadStart(Progress)) { IsBackground = true };
            threadProcessor.Start();
        }

        public void Dispose()
        { 
        
        }

        private void Progress()
        {
            //Stopwatch swSleep = new Stopwatch();
            //TimeSpan ts = new TimeSpan(0, 0, 0, 0, 1);
            //swSleep.Start();

            //2023.04.25 ::: 내부 인터락 때문에 초기 풀고 시작 합니다.
            //Set_StateMode(EModeMatrix.ManualMode_Matrix);
            Setup_StateMatrix(EModeMatrix.ManualMode_Matrix);
            eCommandResult = EReturn.StateCommand_OK;
            //iArray_CF_Function_State_Number = 0;

            

            while (true)
            {
                Thread.Sleep(1);
                //Current State Check
                if (eCurrCommand != ePrevCommand && eCurrCommand != Command.CurrentState_None)
                {
                    //Set_StateMatrix((Command)eCurrCommand, out EReturn eReturn);
                    //eCommandResult = eReturn;

                    if (Set_StateMatrix((Command)eCurrCommand, out EReturn eReturn) && eReturn == EReturn.StateCommand_OK)
                    {
                        ePrevCommand = eCurrCommand;
                        eCommandResult = eReturn;

                        //Console.WriteLine($"{DateTime.Now} >>> Action Command {eCurrState} {eCurrModeMatrix}");
                    }
                    //iArray_CF_Function_State_Number = 0;
                }
                //Current State Run
                if (eCommandResult == EReturn.StateCommand_OK && eCurrState != ePrevState)
                {
                    ePrevState = eCurrState;
                    swProcessTime.Restart();
                    eCF_Function_Run_Flag = EProcStep.CF_Function_Run_Flag_BUSY;
                }
                //Process Run
                if (eCF_Function_Run_Flag == EProcStep.CF_Function_Run_Flag_BUSY)
                {
                    try
                    {
                        CurrentState_Action(eCurrState, eCurrModeMatrix);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);

                        devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                           devJace.Program.ELogLevel.Fatal,
                           $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                           $"| CurrentState_Action : {ex.Message}");
                    }
                    

                    //mResetEvent.WaitOne();

                    if (eCF_Function_Run_Flag == EProcStep.CF_Function_Run_Flag_COMPLETE)
                    {
                        swProcessTime.Stop();
                        strCF_Function_Run_Time = $"{swProcessTime.Elapsed.Hours:00}" +
                            $":{swProcessTime.Elapsed.Minutes:00}" +
                            $":{swProcessTime.Elapsed.Seconds:00}" +
                            $".{swProcessTime.Elapsed.Milliseconds:000}";

                        bool IsCurrentStateChanged = false;
                        if (iArray_StateCompleteModeMatrix[(int)eCurrState] != 0)
                        {
                            eCurrState = (ECurrState)iArray_StateCompleteModeMatrix[(int)eCurrState];
                            IsCurrentStateChanged = true;
                            //Console.WriteLine($"{DateTime.Now} >>> Complted Command {eCurrState} {eCurrModeMatrix}");
                        }
                        devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), 
                            devJace.Program.ELogLevel.Info,
                            $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                            $"| IsCurrentStateChanged : {IsCurrentStateChanged} | {swProcessTime.ElapsedMilliseconds:000.0000}ms");
                    }

                    //2022.12.28 ::: temp del
                    //swProcessTime.Stop();
                    //strCF_Function_Run_Time = $"{swProcessTime.Elapsed.Hours:00}" +
                    //    $":{swProcessTime.Elapsed.Minutes:00}" +
                    //    $":{swProcessTime.Elapsed.Seconds:00}" +
                    //    $".{swProcessTime.Elapsed.Milliseconds:000}";
                    //swProcessTime.Restart();
                    //if (iArray_StateCompleteModeMatrix[(int)eCurrState] != 0)
                    //{
                    //    eCurrState = (ECurrState)iArray_StateCompleteModeMatrix[(int)eCurrState];
                    //}
                    TimeSpan tsProcessTime = DateTime.Now - processTime;
                    processTime = DateTime.Now;

                    //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                    //    $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                    //    $"| Normal | {tsProcessTime.TotalMilliseconds}ms");

                    if (tsProcessTime.TotalMilliseconds >= 100)
                    {
                        devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Warn,
                         $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                         $"| Normal | {tsProcessTime.TotalMilliseconds:000.0000}ms");
                    }

                    
                }
                Contiused_Action(eCurrState, eCurrModeMatrix);
            }
        }

        public void Recv_Action_Complted()
        {
            eCF_Function_Run_Flag = EProcStep.CF_Function_Run_Flag_COMPLETE;
        }

        public void Set_State_Matrix(Command stateCommand)
        {
            if (eCurrCommand != stateCommand)
            {
                eCurrCommand = stateCommand;
            }
        }

        public void Set_StateMode(EModeMatrix eMode)
        {
            try
            {
                //한번만 명령 입력 되고, 
                if (eCurrModeMatrix != eMode && (eCurrState == ECurrState.CurrentState_Idle || eCurrState == ECurrState.CurrentState_Stopped))
                {
                    //mResetEvent.Set();
                    ePrevModeMatrix = eCurrModeMatrix = eMode;
                    Setup_StateMatrix(eMode);
                    AckNormal_Action(eCurrState, eCurrModeMatrix);
                    //mResetEvent.Reset();
                }
            }
            catch
            {
                ePrevModeMatrix = EModeMatrix.None_Matrix;
            }
        }

        private void Setup_StateMatrix(EModeMatrix modeMatrix)
        {
            //=========================================================================
            // 개정이력
            //=========================================================================
            // 2021.10.14
            // 정명재
            // Setup_StateMatrix(EModeMatrix modeMatrix)
            //=========================================================================

            iArray_CurrentStateModeMatrix = new int[iCurrentState_MaxCount];//현재상태모드 2차원 matrix 초기화
            iArray_StateCompleteModeMatrix = new int[iCurrentState_MaxCount];//현재 상태완료 후 다음 상태 실행 정의를 위한 배열 초기화

            switch (modeMatrix)
            {
                case EModeMatrix.AutomaticMode_Matrix:
                    #region AutomaticMode_Matrix
                    //2차원 matrix 셋업=================================================================================================
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_None] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Idle] = (int)EStateCommand.StateCommand_START | (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Starting] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Excute] = (int)EStateCommand.StateCommand_COMPLETE | (int)EStateCommand.StateCommand_HOLD | (int)EStateCommand.StateCommand_SUSPEND | (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Completing] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Complete] = (int)EStateCommand.StateCommand_RESET | (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Resetting] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Holding] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Hold] = (int)EStateCommand.StateCommand_UNHOLD | (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Unholding] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Suspending] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Suspended] = (int)EStateCommand.StateCommand_UNSUSPEND | (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Unsuspend] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Stopping] = (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Stopped] = (int)EStateCommand.StateCommand_RESET | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Aborting] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Aborted] = (int)EStateCommand.StateCommand_CLEAR;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Clearing] = (int)EStateCommand.StateCommand_ABORT;

                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_None] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Idle] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Starting] = (int)ECurrState.CurrentState_Excute;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Excute] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Completing] = (int)ECurrState.CurrentState_Complete;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Complete] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Resetting] = (int)ECurrState.CurrentState_Idle;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Holding] = (int)ECurrState.CurrentState_Hold;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Hold] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Unholding] = (int)ECurrState.CurrentState_Excute;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Suspending] = (int)ECurrState.CurrentState_Suspended;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Suspended] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Unsuspend] = (int)ECurrState.CurrentState_Excute;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Stopping] = (int)ECurrState.CurrentState_Stopped;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Stopped] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Aborting] = (int)ECurrState.CurrentState_Aborted;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Aborted] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Clearing] = (int)ECurrState.CurrentState_Stopped;
                    #endregion
                    break;

                case EModeMatrix.IdleMode_Matrix:
                    #region IdleMode_Matrix
                    //2차원 matrix 셋업=================================================================================================
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_None] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Idle] = (int)EStateCommand.StateCommand_START | (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Starting] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Excute] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Completing] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Complete] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Resetting] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Holding] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Hold] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Unholding] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Suspending] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Suspended] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Unsuspend] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Stopping] = (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Stopped] = (int)EStateCommand.StateCommand_RESET | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Aborting] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Aborted] = (int)EStateCommand.StateCommand_CLEAR;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Clearing] = (int)EStateCommand.StateCommand_ABORT;

                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_None] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Idle] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Starting] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Excute] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Completing] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Complete] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Resetting] = (int)ECurrState.CurrentState_Idle;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Holding] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Hold] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Unholding] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Suspending] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Suspended] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Unsuspend] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Stopping] = (int)ECurrState.CurrentState_Stopped;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Stopped] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Aborting] = (int)ECurrState.CurrentState_Aborted;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Aborted] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Clearing] = (int)ECurrState.CurrentState_Stopped;
                    #endregion
                    break;

                case EModeMatrix.MaintenanceMode_Matrix:
                    #region MaintenanceMode_Matrix
                    //2차원 matrix 셋업=================================================================================================
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_None] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Idle] = (int)EStateCommand.StateCommand_START | (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Starting] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Excute] = (int)EStateCommand.StateCommand_COMPLETE | (int)EStateCommand.StateCommand_HOLD | (int)EStateCommand.StateCommand_SUSPEND | (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Completing] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Complete] = (int)EStateCommand.StateCommand_RESET | (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Resetting] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Holding] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Hold] = (int)EStateCommand.StateCommand_UNHOLD | (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Unholding] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Suspending] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Suspended] = (int)EStateCommand.StateCommand_UNSUSPEND | (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Unsuspend] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Stopping] = (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Stopped] = (int)EStateCommand.StateCommand_RESET | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Aborting] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Aborted] = (int)EStateCommand.StateCommand_CLEAR;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Clearing] = (int)EStateCommand.StateCommand_ABORT;

                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_None] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Idle] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Starting] = (int)ECurrState.CurrentState_Excute;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Excute] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Completing] = (int)ECurrState.CurrentState_Complete;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Complete] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Resetting] = (int)ECurrState.CurrentState_Idle;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Holding] = (int)ECurrState.CurrentState_Hold;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Hold] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Unholding] = (int)ECurrState.CurrentState_Excute;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Suspending] = (int)ECurrState.CurrentState_Suspended;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Suspended] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Unsuspend] = (int)ECurrState.CurrentState_Excute;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Stopping] = (int)ECurrState.CurrentState_Stopped;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Stopped] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Aborting] = (int)ECurrState.CurrentState_Aborted;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Aborted] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Clearing] = (int)ECurrState.CurrentState_Stopped;
                    #endregion
                    break;

                case EModeMatrix.ManualMode_Matrix:
                    #region ManualMode_Matrix
                    //2차원 matrix 셋업=================================================================================================
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_None] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Idle] = (int)EStateCommand.StateCommand_START | (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Starting] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Excute] = (int)EStateCommand.StateCommand_COMPLETE | (int)EStateCommand.StateCommand_HOLD | (int)EStateCommand.StateCommand_SUSPEND | (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Completing] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Complete] = (int)EStateCommand.StateCommand_RESET | (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Resetting] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Holding] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Hold] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Unholding] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Suspending] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Suspended] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Unsuspend] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Stopping] = (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Stopped] = (int)EStateCommand.StateCommand_RESET | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Aborting] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Aborted] = (int)EStateCommand.StateCommand_CLEAR;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Clearing] = (int)EStateCommand.StateCommand_ABORT;

                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_None] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Idle] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Starting] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Excute] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Completing] = (int)ECurrState.CurrentState_Complete;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Complete] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Resetting] = (int)ECurrState.CurrentState_Idle;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Holding] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Hold] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Unholding] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Suspending] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Suspended] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Unsuspend] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Stopping] = (int)ECurrState.CurrentState_Stopped;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Stopped] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Aborting] = (int)ECurrState.CurrentState_Aborted;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Aborted] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Clearing] = (int)ECurrState.CurrentState_Stopped;
                    #endregion
                    break;

                case EModeMatrix.SemiAutomaticMode_Matrix:
                    #region SemiAutomaticMode_Matrix
                    //2차원 matrix 셋업=================================================================================================
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_None] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Idle] = (int)EStateCommand.StateCommand_START | (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Starting] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Excute] = (int)EStateCommand.StateCommand_COMPLETE | (int)EStateCommand.StateCommand_HOLD | (int)EStateCommand.StateCommand_SUSPEND | (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Completing] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Complete] = (int)EStateCommand.StateCommand_RESET | (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Resetting] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Holding] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Hold] = (int)EStateCommand.StateCommand_UNHOLD | (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Unholding] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Suspending] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Suspended] = (int)EStateCommand.StateCommand_UNSUSPEND | (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Unsuspend] = (int)EStateCommand.StateCommand_STOP | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Stopping] = (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Stopped] = (int)EStateCommand.StateCommand_RESET | (int)EStateCommand.StateCommand_ABORT;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Aborting] = (int)EStateCommand.StateCommand_NONE;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Aborted] = (int)EStateCommand.StateCommand_CLEAR;
                    iArray_CurrentStateModeMatrix[(int)ECurrState.CurrentState_Clearing] = (int)EStateCommand.StateCommand_ABORT;

                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_None] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Idle] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Starting] = (int)ECurrState.CurrentState_Excute;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Excute] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Completing] = (int)ECurrState.CurrentState_Complete;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Complete] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Resetting] = (int)ECurrState.CurrentState_Idle;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Holding] = (int)ECurrState.CurrentState_Hold;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Hold] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Unholding] = (int)ECurrState.CurrentState_Excute;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Suspending] = (int)ECurrState.CurrentState_Suspended;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Suspended] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Unsuspend] = (int)ECurrState.CurrentState_Excute;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Stopping] = (int)ECurrState.CurrentState_Stopped;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Stopped] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Aborting] = (int)ECurrState.CurrentState_Aborted;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Aborted] = (int)ECurrState.CurrentState_None;
                    iArray_StateCompleteModeMatrix[(int)ECurrState.CurrentState_Clearing] = (int)ECurrState.CurrentState_Stopped;
                    #endregion
                    break;
            }

            IsIntanceAction = true;
        }

        private bool Set_StateMatrix(Command stateCommand, out EReturn result)
        {
            //=========================================================================
            // 개정이력
            //=========================================================================
            // 2021.10.14
            // 정명재
            // Set_StateMatrix(ECurrState stateCommand, out EReturn result)
            //=========================================================================

            //[Index ::: jace]
            bool rtn = false;
            result = EReturn.StateCommand_NG;

            if (dicCommand.TryGetValue(stateCommand, out EStateCommand command) == true)
            {
                //if ((iArray_CurrentStateModeMatrix[(int)stateCommand] & (int)command) != 0)
                //{
                //    result = EReturn.StateCommand_OK;
                //    eCurrState = (ECurrState)stateCommand;
                //}

                if ((iArray_CurrentStateModeMatrix[(int)eCurrState] & (int)command) != 0)
                {
                    result = EReturn.StateCommand_OK;
                    eCurrState = (ECurrState)stateCommand;
                    AckNormal_Action(eCurrState, eCurrModeMatrix);
                }
                //ePrevCommand = eCurrCommand;
                //2022.12.30
                //eCurrCommand = stateCommand;
                rtn = true;
            }
            return rtn;

            #region MyRegion
            /*
               switch (stateCommand)
               {
                   default:                    
                       break;

                   case ECurrState.CurrentState_Aborting:
                       if ((iArray_CurrentStateModeMatrix[iCurrentState] & (int)EStateCommand.StateCommand_ABORT) != 0)
                       {
                           result = EReturn.StateCommand_OK;
                           iCurrentState = (int)stateCommand;
                       }
                       break;

                   case ECurrState.CurrentState_Clearing:
                       if ((iArray_CurrentStateModeMatrix[iCurrentState] & (int)EStateCommand.StateCommand_CLEAR) != 0)
                       {
                           result = EReturn.StateCommand_OK;
                           iCurrentState = (int)stateCommand;
                       }
                       break;

                   case ECurrState.CurrentState_Completing:
                       if ((iArray_CurrentStateModeMatrix[iCurrentState] & (int)EStateCommand.StateCommand_COMPLETE) != 0)
                       {
                           result = EReturn.StateCommand_OK;
                           iCurrentState = (int)stateCommand;
                       }
                       break;

                   case ECurrState.CurrentState_Holding:
                       if ((iArray_CurrentStateModeMatrix[iCurrentState] & (int)EStateCommand.StateCommand_HOLD) != 0)
                       {
                           result = EReturn.StateCommand_OK;
                           iCurrentState = (int)stateCommand;
                       }
                       break;

                   case ECurrState.CurrentState_Resetting:
                       if ((iArray_CurrentStateModeMatrix[iCurrentState] & (int)EStateCommand.StateCommand_RESET) != 0)
                       {
                           result = EReturn.StateCommand_OK;
                           iCurrentState = (int)stateCommand;
                       }
                       break;

                   case ECurrState.CurrentState_Starting:
                       if ((iArray_CurrentStateModeMatrix[iCurrentState] & (int)EStateCommand.StateCommand_START) != 0)
                       {
                           result = EReturn.StateCommand_OK;
                           iCurrentState = (int)stateCommand;
                       }
                       break;

                   case ECurrState.CurrentState_Stopping:
                       if ((iArray_CurrentStateModeMatrix[iCurrentState] & (int)EStateCommand.StateCommand_STOP) != 0)
                       {
                           result = EReturn.StateCommand_OK;
                           iCurrentState = (int)stateCommand;
                       }
                       break;

                   case ECurrState.CurrentState_Suspended:
                       if ((iArray_CurrentStateModeMatrix[iCurrentState] & (int)EStateCommand.StateCommand_UNSUSPEND) != 0)
                       {
                           result = EReturn.StateCommand_OK;
                           iCurrentState = (int)stateCommand;
                       }
                       break;             

                   case ECurrState.CurrentState_Suspending:
                       if ((iArray_CurrentStateModeMatrix[iCurrentState] & (int)EStateCommand.StateCommand_SUSPEND) != 0)
                       {
                           result = EReturn.StateCommand_OK;
                           iCurrentState = (int)stateCommand;
                       }
                       break;

                   case ECurrState.CurrentState_Unholding:
                       if ((iArray_CurrentStateModeMatrix[iCurrentState] & (int)EStateCommand.StateCommand_UNHOLD) != 0)
                       {
                           result = EReturn.StateCommand_OK;
                           iCurrentState = (int)stateCommand;
                       }
                       break;
               }
               */
            #endregion

        }
    }

    public enum EReturn
    {
        StateCommand_NG = 0,
        StateCommand_OK = 1,
    }

    public enum EModeMatrix
    {
        None_Matrix = 0,
        AutomaticMode_Matrix = 1,
        ManualMode_Matrix = 2,
        MaintenanceMode_Matrix = 3,
        SemiAutomaticMode_Matrix = 4,
        IdleMode_Matrix = 5,
    }

    public enum ECurrState
    {
        //실행중인 상태 정의
        CurrentState_None = 00,
        CurrentState_Idle = 01,
        CurrentState_Starting = 02,
        CurrentState_Excute = 03,
        CurrentState_Completing = 04,
        CurrentState_Complete = 05,
        CurrentState_Resetting = 06,
        CurrentState_Holding = 07,
        CurrentState_Hold = 08,
        CurrentState_Unholding = 09,
        CurrentState_Suspending = 10,
        CurrentState_Suspended = 11,
        CurrentState_Unsuspend = 12,
        CurrentState_Stopping = 13,
        CurrentState_Stopped = 14,
        CurrentState_Aborting = 15,
        CurrentState_Aborted = 16,
        CurrentState_Clearing = 17,
    }

    public enum Command
    {
        CurrentState_None = 00,
        CurrentState_Starting = 02,
        CurrentState_Completing = 04,
        CurrentState_Resetting = 06,
        CurrentState_Holding = 07,
        CurrentState_Unholding = 09,
        CurrentState_Suspending = 10,
        CurrentState_Suspended = 11,
        CurrentState_Stopping = 13,
        CurrentState_Aborting = 15,
        CurrentState_Clearing = 17,
    }

    enum EStateCommand
    {
        //상태명령 정의 
        StateCommand_NONE = 0x000,
        StateCommand_START = 0x001,
        StateCommand_COMPLETE = 0x002,
        StateCommand_RESET = 0x004,
        StateCommand_HOLD = 0x008,
        StateCommand_UNHOLD = 0x010,
        StateCommand_SUSPEND = 0x020,
        StateCommand_UNSUSPEND = 0x040,
        StateCommand_CLEAR = 0x080,
        StateCommand_STOP = 0x100,
        StateCommand_ABORT = 0x200,

    }  

    public enum EProcStep
    {
        CF_Function_Run_Flag_IDLE = 000,
        CF_Function_Run_Flag_BUSY = 100,
        CF_Function_Run_Flag_COMPLETE = 200,
    }

}
