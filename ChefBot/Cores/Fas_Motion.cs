using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using FASTECH;
using System.Net;
using System.Threading;
using devi;

namespace Cores
{
    class Fas_Motion
    {
        public const int TCP = 0;
        public const int UDP = 1;

        public const int INPUTPIN = 12;
        public const byte OUTPUTPIN = 10;

        public static int nCnt = 0;

        public const int SLAVE_CNT = 2;

        public const int CW = 1;
        public const int CCW = 0;

        public const int INCPOS = 200000; // IncMove Target Position
        public const int ABSPOS = 0; // AbsMove Target Position

        private static int iPrevAccdecTime = 0;

        #region 01.ConnectionExam
        /// <summary>
        /// Connection
        /// </summary>
        /// <param name="nCommType"></param>
        /// <param name="nBdID"></param>
        /// <returns></returns>
        public static bool Connect(string ipAddress, int nBdID)
        {
            //IPAddress ip = new IPAddress(new byte[] { 192, 168, 0, 2 }); // IP : 192.168.0.2

            IPAddress ip = IPAddress.Parse(ipAddress);
            bool bSuccess = true;
            int nCommType = TCP;
            // Connection
            switch (nCommType)
            {
                case TCP:
                    // TCP Connection
                    if (EziMOTIONPlusELib.FAS_ConnectTCP(ip, nBdID) == false)
                    {
                        //Console.WriteLine("TCP Connection Fail!");
                        bSuccess = false;
                    }
                    break;

                case UDP:
                    // UDP Connection
                    if (EziMOTIONPlusELib.FAS_Connect(ip, nBdID) == false)
                    {
                        Console.WriteLine("UDP Connection Fail!");
                        bSuccess = false;
                    }
                    break;

                default:
                    Console.WriteLine("Wrong communication type.");
                    bSuccess = false;

                    break;
            }

            if (bSuccess)
                Console.WriteLine("Connected successfully.");

            return bSuccess;
        }

        /// <summary>
        /// Stepper Info
        /// </summary>
        /// <param name="nBdID"></param>
        /// <returns></returns>
        public static bool CheckDriveInfo(int nBdID, out string info)
        {
            byte byType = 0;
            string version = "";
            info = null;
            int nRtn;

            // Read Drive's information
            nRtn = EziMOTIONPlusELib.FAS_GetSlaveInfo(nBdID, ref byType, ref version);
            if (nRtn != EziMOTIONPlusELib.FMM_OK)
            {
                //Console.WriteLine("Function(FAS_GetSlaveInfo) was failed.");
                info = "Function(FAS_GetSlaveInfo) was failed.";
                return false;
            }

            info = string.Format("Board ID {0} : TYPE= {1}, Version= {2}", nBdID, byType, version);
            //Console.WriteLine("Board ID {0} : TYPE= {1}, Version= {2}", nBdID, byType, version);

            return true;
        }
        #endregion

        #region 02.ParameterTestExam
        static bool SetParameter(int nBdID)
        {
            int nRtn;
            int nChangeValue = 100;
            int lParamVal = 0;

            Console.WriteLine("-----------------------------------------------------------------------------");
            // Check The Axis Start Speed Parameter Status
            nRtn = EziMOTIONPlusELib.FAS_GetParameter(nBdID, 2 /*SERVO2_AXISSTARTSPEED*/, ref lParamVal);
            if (nRtn != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_GetParameter) was failed.");
                return false;
            }
            else
            {
                Console.WriteLine("Load Parameter[Before] : Start Speed = {0}[pps]", lParamVal);
            }

            Console.WriteLine("-----------------------------------------------------------------------------");
            // Change the (Axxis Start Speed Parameter) vlaue to (nChangeValue) value.
            nRtn = EziMOTIONPlusELib.FAS_SetParameter(nBdID, 2 /*SERVO2_AXISSTARTSPEED*/, nChangeValue);
            if (nRtn != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetParameter) was failed.");
                return false;
            }
            else
            {
                Console.WriteLine("Set Parameter: Start Speed = {0}[pps]", nChangeValue);
            }

            Console.WriteLine("-----------------------------------------------------------------------------");
            // Check the changed Axis Start Speed Parameter again.
            nRtn = EziMOTIONPlusELib.FAS_GetParameter(nBdID, 2 /*SERVO2_AXISSTARTSPEED*/, ref lParamVal);
            if (nRtn != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_GetParameter) was failed.");
                return false;
            }
            else
            {
                Console.WriteLine("Load Parameter[After] : Start Speed = {0}[pps]", lParamVal);
            }

            return true;
        }
        #endregion

        #region 03.InputPinControlExam
        static bool SetInputPin(int nBdID)
        {
            byte byPinNo = 0;
            uint dwLogicMask = 0;
            byte byLevel = 0;

            uint dwInputMask = 0;

            Console.WriteLine("\n-----------------------------------------------------------------------------");

            // Set Input pin Value.
            byPinNo = 3;
            byLevel = EziMOTIONPlusELib.LEVEL_LOW_ACTIVE;
            dwInputMask = 0x04000000; // SERVO2_IN_BITMASK_USERIN0

            if (EziMOTIONPlusELib.FAS_SetIOAssignMap(nBdID, byPinNo, dwInputMask, byLevel) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetIOAssignMap) was failed.");
                return false;
            }
            else
            {
                Console.WriteLine("SERVO2_IN_BITMASK_USERIN0 (Pin{0}) : [{1}]", byPinNo, ((byLevel == EziMOTIONPlusELib.LEVEL_LOW_ACTIVE) ? "Low Active" : "High Active"));
            }

            Console.WriteLine("\n-----------------------------------------------------------------------------");

            // Show Input pins status
            for (int i = 0; i < INPUTPIN; i++)
            {
                if (EziMOTIONPlusELib.FAS_GetIOAssignMap(nBdID, (byte)i, ref dwLogicMask, ref byLevel) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_SetIOAssignMap) was failed.");
                    return false;
                }

                if (dwLogicMask != EziMOTIONPlusELib.IN_LOGIC_NONE)
                    Console.WriteLine("Input PIN[{0}] : Logic Mask 0x{1:X8} ({2})", i, dwLogicMask, ((byLevel == EziMOTIONPlusELib.LEVEL_LOW_ACTIVE) ? "Low Active" : "High Active"));
                else
                    Console.WriteLine("Input Pin[{0}] : Not Assigned", i);
            }

            return true;
        }

        static bool CheckInputSignal(int nBdID)
        {
            uint dwInput = 0;
            uint dwInputMask = 0x04000000; // SERVO2_IN_BITMASK_USERIN0

            Console.WriteLine("\n---------------------------Input Monitoring Start--------------------------");

            // When the value SERVO2_IN_BITMASK_USERIN0 is entered with the set PinNo, an input confirmation message is displyed.
            // Monitoring input signal while 60 seconds
            do
            {
                if (EziMOTIONPlusELib.FAS_GetIOInput(nBdID, ref dwInput) == EziMOTIONPlusELib.FMM_OK)
                {
                    if (((dwInput & dwInputMask) != 0 ? true : false))
                    {
                        Console.WriteLine("INPUT PIN DETECTED.");
                    }
                }
                else
                    return false;
            } while (WaitSeconds(60));

            return true;
        }

        /// <summary>
        /// SetInputPin, SetOutputPin
        /// </summary>
        /// <param name="nSecond"></param>
        /// <returns></returns>
        static bool WaitSeconds(int nSecond)
        {
            Thread.Sleep(100);

            //static int nCnt = 0;
            if (nCnt++ < nSecond * 10)
                return true;
            else
                return false;
        }
        #endregion

        #region 04.OutputPinControlExam
        static bool SetOutputPin(int nBdID)
        {
            byte byPinNo = 0;
            byte byLevel = 0;

            uint dwLogicMask = 0;
            uint dwOutputMask = 0;

            Console.WriteLine("\n-----------------------------------------------------------------------------");
            // Check OutputPin Status
            for (int i = 0; i < OUTPUTPIN; i++)
            {
                if (EziMOTIONPlusELib.FAS_GetIOAssignMap(nBdID, (byte)(INPUTPIN + i), ref dwLogicMask, ref byLevel) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetIOAssignMap) was failed.");
                    return false;
                }

                if (dwLogicMask != EziMOTIONPlusELib.IN_LOGIC_NONE)
                    Console.WriteLine("Output PIN[{0}] : Logic Mask 0x{1:X8} ({2})", i, dwLogicMask, ((byLevel == EziMOTIONPlusELib.LEVEL_LOW_ACTIVE) ? "Low Active" : "High Active"));
                else
                    Console.WriteLine("Output Pin[{0}] : Not Assigned", i);
            }

            Console.WriteLine("\n-----------------------------------------------------------------------------");

            // Set Output pin Value.
            byPinNo = 3;
            byLevel = EziMOTIONPlusELib.LEVEL_HIGH_ACTIVE;
            dwOutputMask = 0x00008000; // SERVO2_OUT_BITMASK_USEROUT0

            if (EziMOTIONPlusELib.FAS_SetIOAssignMap(nBdID, (byte)(INPUTPIN + byPinNo), dwOutputMask, byLevel) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetIOAssignMap) was failed.");
                return false;
            }

            // Show Output pins status
            for (int i = 0; i < OUTPUTPIN; i++)
            {
                if (EziMOTIONPlusELib.FAS_GetIOAssignMap(nBdID, (byte)(INPUTPIN + i), ref dwLogicMask, ref byLevel) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetIOAssignMap) was failed.");
                    return false;
                }

                if (dwLogicMask != EziMOTIONPlusELib.IN_LOGIC_NONE)
                    Console.WriteLine("Output PIN[{0}] : Logic Mask 0x{1:X8} ({2})", i, dwLogicMask, ((byLevel == EziMOTIONPlusELib.LEVEL_LOW_ACTIVE) ? "Low Active" : "High Active"));
                else
                    Console.WriteLine("Output Pin[{0}] : Not Assigned", i);
            }

            return true;
        }

        static bool ControlOutputSignal(int nBdID)
        {
            uint dwOutputMask = dwOutputMask = 0x00008000; // SERVO2_OUT_BITMASK_USEROUT0

            Console.WriteLine("\n-----------------------------------------------------------------------------");

            // Control output signal on and off for 60 seconds
            do
            {
                Thread.Sleep(1000);
                // USEROUT0: ON
                if (EziMOTIONPlusELib.FAS_SetIOOutput(nBdID, dwOutputMask, 0) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_SetIOOutput) was failed.");
                    return false;
                }

                Thread.Sleep(1000);
                // USEROUT0: OFF
                if (EziMOTIONPlusELib.FAS_SetIOOutput(nBdID, 0, dwOutputMask) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_SetIOOutput) was failed.");
                    return false;
                }
            } while (WaitSeconds(60));

            return true;
        }
        #endregion

        #region 05.JogMovmentExam

        /// <summary>
        /// 05.JogMovmentExam, 06.JogMovementExEmam
        /// </summary>
        /// <param name="nBdID"></param>
        /// <returns></returns>
        public static bool CheckDriveErr(int nBdID)
        {
            // Check Drive's Error
            uint dwAxisStatus = 0;
            bool flagErr = false;

            if (EziMOTIONPlusELib.FAS_GetAxisStatus(nBdID, ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_GetAxisStatus) was failed.");
                return false;
            }

            flagErr = (dwAxisStatus & 0x00000001 /*FFLAG_ERRORALL*/ ) != 0 ? true : false;
            if (flagErr)
            {
                // if Drive's Error was detected, Reset the ServoAlarm
                if (EziMOTIONPlusELib.FAS_ServoAlarmReset(nBdID) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_ServoAlarmReset) was failed.");
                    return false;
                }
            }

            return true;
        }

        public static bool SetServoOn(int nBdID, int nOnOff)
        {
            // Check Drive's Servo Status
            uint dwAxisStatus = 0;
            bool flagServOn = false;

            // if ServoOnFlagBit is OFF('0'), switch to ON('1')

            if (EziMOTIONPlusELib.FAS_GetAxisStatus(nBdID, ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_GetAxisStatus) was failed.");
                return false;
            }

            flagServOn = (dwAxisStatus & 0x00100000 /*FFLAG_SERVOON*/ ) != 0 ? true : false;  
            
            if (flagServOn == false)
            {
                if (EziMOTIONPlusELib.FAS_ServoEnable(nBdID, nOnOff) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_ServoEnable) was failed.");
                    return false;
                }

                Thread.Sleep(1);

                if (EziMOTIONPlusELib.FAS_GetAxisStatus(nBdID, ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetAxisStatus) was failed.");
                    return false;
                }

                flagServOn = (dwAxisStatus & 0x00100000 /*FFLAG_SERVOON*/ ) != 0 ? true : false;
                if (flagServOn == true)
                    Console.WriteLine("Servo ON");

                //2022.12.29 ::: 멈춤 증상 으로 제거
                //do
                //{
                //    Thread.Sleep(1);
                //
                //    if (EziMOTIONPlusELib.FAS_GetAxisStatus(nBdID, ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
                //    {
                //        Console.WriteLine("Function(FAS_GetAxisStatus) was failed.");
                //        return false;
                //    }
                //
                //    flagServOn = (dwAxisStatus & 0x00100000 /*FFLAG_SERVOON*/ ) != 0 ? true : false;
                //    if (flagServOn == true)
                //        Console.WriteLine("Servo ON");
                //} while (!flagServOn); // Wait until FFLAG_SERVOON is ON
            }
            else
            {
                if (nOnOff == 1)
                {
                    Console.WriteLine("Servo is already ON");
                }
                else
                {
                    if (EziMOTIONPlusELib.FAS_ServoEnable(nBdID, nOnOff) != EziMOTIONPlusELib.FMM_OK)
                    {
                        Console.WriteLine("Function(FAS_ServoEnable) was failed.");
                        return false;
                    }
                }
            }

            return true;
        }

        static bool JogMove(int nBdID, int nDir)
        {
            // Jogmode Move 
            uint nTargetVeloc = 10000;
            int nDirect = nDir;
            int nSeconds = 5 * 1000; // Wait 5 Sec

            uint dwAxisStatus = 0;
            bool flagMontion = false;

            Console.WriteLine("---------------------------");
            if (EziMOTIONPlusELib.FAS_MoveVelocity(nBdID, nTargetVeloc, nDirect) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_MoveVelocity) was failed.");
                return false;
            }

            Console.WriteLine("Move Motor!");

            Thread.Sleep(nSeconds);

            if (EziMOTIONPlusELib.FAS_MoveStop(nBdID) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_MoveStop) was failed.");
                return false;
            }

            do
            {
                // Wait until FFLAG_MOTIONING is OFF
                Thread.Sleep(1);

                if (EziMOTIONPlusELib.FAS_GetAxisStatus(nBdID, ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetAxisStatus) was failed.");
                    return false;
                }

                flagMontion = (dwAxisStatus & 0x08000000 /*FFLAG_MOTIONING*/ ) != 0 ? true : false;
                if (flagMontion == false)
                {
                    Console.WriteLine("Move Stop!");
                }
            } while (flagMontion);

            return true;
        }


        #endregion

        #region 06.JogMovementExEmam
        public static bool JogMove(int nBdID, int nDir, int nAccDecTime, int nCmdVeloc)
        {
            // SetParameter & MoveVelocity
            uint nTargetVeloc = (uint)Math.Abs(nCmdVeloc);
            int nDirect = nDir;

            if (iPrevAccdecTime != nAccDecTime)
            {
                iPrevAccdecTime = nAccDecTime;
                // Set Jog Acc/Dec Time
                if (EziMOTIONPlusELib.FAS_SetParameter(nBdID, /*SERVO2_JOGACCDECTIME*/8, nAccDecTime) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_SetParameter) was failed.");
                    return false;
                }

                //Console.WriteLine("---------------------------");
               
            }

            if (EziMOTIONPlusELib.FAS_MoveVelocity(nBdID, nTargetVeloc, nDirect) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_MoveVelocity) was failed.");
                return false;
            }         

            return true;
        }

        static bool JogExMove(int nBdID, int nAccDecTime)
        {
            // MoveVelocityEx

            // Set velocity
            int nDirect = 1;
            uint lVelocity = 30000;
            int nSeconds = 3 * 1000; // Wait 3 Sec

            uint dwAxisStatus = 0;
            bool flagMontion = false;
            EziMOTIONPlusELib.VELOCITY_OPTION_EX opt = new EziMOTIONPlusELib.VELOCITY_OPTION_EX();

            // set user setting bit(BIT_USE_CUSTOMACCDEC) and acel/decel time value
            opt.BIT_USE_CUSTOMACCDEC = true;
            opt.wCustomAccDecTime = (ushort)nAccDecTime;

            Console.WriteLine("-----------------------------------------------------------\n");
            if (EziMOTIONPlusELib.FAS_MoveVelocityEx(nBdID, lVelocity, nDirect, opt) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_MoveVelocityEx) was failed.");
                return false;
            }

            Console.WriteLine("Move Motor(Jog Ex Mode)!");

            Thread.Sleep(nSeconds);

            if (EziMOTIONPlusELib.FAS_MoveStop(nBdID) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_MoveStop) was failed.");
                return false;
            }

            do
            {
                // Wait until FFLAG_MOTIONING is OFF
                Thread.Sleep(1);

                if (EziMOTIONPlusELib.FAS_GetAxisStatus(nBdID, ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetAxisStatus) was failed.");
                    return false;
                }

                flagMontion = (dwAxisStatus & 0x08000000 /*FFLAG_MOTIONING*/ ) != 0 ? true : false;
                if (flagMontion == false)
                {
                    Console.WriteLine("Move Stop!");
                }
            } while (flagMontion);

            return true;
        }

        #endregion

        #region 07.MoveAbsIncPosExam
        static bool MoveIncPos(int nBdID)
        {
            // Move AxisIncPos
            uint dwAxisStatus = 0;
            bool flagMotion = false;
            bool flagInpos = false;
            int lIncPos;
            uint lVelocity = 0;

            Console.WriteLine("---------------------------");
            // Increase the motor by 370000 pulse (target position : Relative position)

            lIncPos = 370000;
            lVelocity = 40000;

            Console.WriteLine("[Inc Mode] Move Motor !");

            if (EziMOTIONPlusELib.FAS_MoveSingleAxisIncPos(nBdID, lIncPos, lVelocity) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_MoveSingleAxisIncPos) was failed.");
                return false;
            }

            // Check the Axis status until motor stops and the Inposition value is checked
            do
            {
                Thread.Sleep(1);

                if (EziMOTIONPlusELib.FAS_GetAxisStatus(nBdID, ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetAxisStatus) was failed.");
                    return false;
                }

                flagMotion = (dwAxisStatus & 0x08000000 /*FFLAG_MOTIONING*/ ) != 0 ? true : false;
                flagInpos = (dwAxisStatus & 0x00080000 /*FFLAG_INPOSITION*/ ) != 0 ? true : false;

            } while (flagMotion || !flagInpos);

            return true;
        }

        static bool MoveAbsPos(int nBdID)
        {
            // Move AxisAbsPos 
            uint dwAxisStatus = 0;
            bool flagMotion = false;
            bool flagInpos = false;
            int lAbsPos;
            uint lVelocity = 0;

            Console.WriteLine("---------------------------");

            // Move the motor by 0 pulse (target position : Absolute position)
            lAbsPos = 0;
            lVelocity = 40000;

            Console.WriteLine("[Abs Mode] Move Motor!");
            if (EziMOTIONPlusELib.FAS_MoveSingleAxisAbsPos(nBdID, lAbsPos, lVelocity) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_MoveSingleAxisAbsPos) was failed.");
                return false;
            }

            // Check the Axis status until motor stops and the Inposition value is checked
            do
            {
                Thread.Sleep(1);

                if (EziMOTIONPlusELib.FAS_GetAxisStatus(nBdID, ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetAxisStatus) was failed.");
                    return false;
                }

                flagMotion = (dwAxisStatus & 0x08000000 /*FFLAG_MOTIONING*/ ) != 0 ? true : false;
                flagInpos = (dwAxisStatus & 0x00080000 /*FFLAG_INPOSITION*/ ) != 0 ? true : false;

            } while (flagMotion || !flagInpos);

            return true;
        }

        public static bool MovePos(int nBdID, int nAbsInc, int nDir, uint lVelocity, int nPos)
        {
            switch (nAbsInc)
            {
                case 0:
                    if (EziMOTIONPlusELib.FAS_MoveSingleAxisAbsPos(nBdID, nPos, lVelocity) != EziMOTIONPlusELib.FMM_OK)
                    {
                        Console.WriteLine("Function(FAS_MoveSingleAxisAbsPos) was failed.");
                        return false;
                    }
                    break;

                case 1:
                    switch (nDir)
                    {
                        case 0:
                            int iRevers = nPos * -1;
                            if (EziMOTIONPlusELib.FAS_MoveSingleAxisIncPos(nBdID, iRevers, lVelocity) != EziMOTIONPlusELib.FMM_OK)
                            {
                                Console.WriteLine("Function(FAS_MoveSingleAxisIncPos) was failed.");
                                return false;
                            }
                            break;
                        case 1:
                            if (EziMOTIONPlusELib.FAS_MoveSingleAxisIncPos(nBdID, nPos, lVelocity) != EziMOTIONPlusELib.FMM_OK)
                            {
                                Console.WriteLine("Function(FAS_MoveSingleAxisIncPos) was failed.");
                                return false;
                            }
                            break;
                        default:
                            break;
                    }
                    
                    break;
                default:
                    break;
            }
            return true;
        }

        public static bool MovePos(int nBdID, int nAbsInc, int nDir, uint lVelocity, int nPos, int nAccDecTime)
        {

            if (iPrevAccdecTime != nAccDecTime)
            {
                iPrevAccdecTime = nAccDecTime;

                // Set Acc Time
                if (EziMOTIONPlusELib.FAS_SetParameter(nBdID, 3 /*SERVO2_AXISACCTIME*/, nAccDecTime) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_SetParameter) was failed.");
                    return false;
                }

                // Set Dec Time
                if (EziMOTIONPlusELib.FAS_SetParameter(nBdID, 4 /*SERVO2_AXISDECTIME*/, nAccDecTime) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_SetParameter) was failed.");
                    return false;
                }
            }
          

            switch (nAbsInc)
            {
                case 0:
                    if (EziMOTIONPlusELib.FAS_MoveSingleAxisAbsPos(nBdID, nPos, lVelocity) != EziMOTIONPlusELib.FMM_OK)
                    {
                        Console.WriteLine("Function(FAS_MoveSingleAxisAbsPos) was failed.");
                        return false;
                    }
                    break;

                case 1:
                    switch (nDir)
                    {
                        case 0:
                            int iRevers = nPos * -1;
                            if (EziMOTIONPlusELib.FAS_MoveSingleAxisIncPos(nBdID, iRevers, lVelocity) != EziMOTIONPlusELib.FMM_OK)
                            {
                                Console.WriteLine("Function(FAS_MoveSingleAxisIncPos) was failed.");
                                return false;
                            }
                            break;
                        case 1:
                            if (EziMOTIONPlusELib.FAS_MoveSingleAxisIncPos(nBdID, nPos, lVelocity) != EziMOTIONPlusELib.FMM_OK)
                            {
                                Console.WriteLine("Function(FAS_MoveSingleAxisIncPos) was failed.");
                                return false;
                            }
                            break;
                        default:
                            break;
                    }

                    break;
                default:
                    break;
            }
            return true;
        }
        #endregion

        #region 08.MoveAbsIncPosExExam
        static bool MovePos(int nBdID, int nDistance, int nAccDecTime)
        {
            // SetParameter & MoveSingleAxisIncPos

            uint dwAxisStatus = 0;
            bool flagMotion = false;
            bool flagInpos = false;
            int lIncPos;
            uint lVelocity;

            // Set Acc Time
            if (EziMOTIONPlusELib.FAS_SetParameter(nBdID, 3 /*SERVO2_AXISACCTIME*/, nAccDecTime) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetParameter) was failed.");
                return false;
            }

            // Set Dec Time
            if (EziMOTIONPlusELib.FAS_SetParameter(nBdID, 4 /*SERVO2_AXISDECTIME*/, nAccDecTime) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetParameter) was failed.");
                return false;
            }

            Console.WriteLine("---------------------------");

            // Move the motor by 100000 pulse (target distance : Relative distance)
            lIncPos = nDistance;
            lVelocity = 40000;

            Console.WriteLine("Move Motor!");
            if (EziMOTIONPlusELib.FAS_MoveSingleAxisIncPos(nBdID, lIncPos, lVelocity) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_MoveSingleAxisIncPos) was failed.");
                return false;
            }

            // Check the Axis status until motor stops and the Inposition value is checked
            do
            {
                Thread.Sleep(1);

                if (EziMOTIONPlusELib.FAS_GetAxisStatus(nBdID, ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetAxisStatus) was failed.");
                    return false;
                }

                flagMotion = (dwAxisStatus & 0x08000000 /*FFLAG_MOTIONING*/ ) != 0 ? true : false;
                flagInpos = (dwAxisStatus & 0x00080000 /*FFLAG_INPOSITION*/ ) != 0 ? true : false;

            } while (flagMotion || !flagInpos);

            return true;
        }

        static bool MovePosEx(int nBdID, int nDistance, int nAccDecTime)
        {
            // MoveSingleAxisIncPosEx
            uint dwAxisStatus = 0;
            bool flagMotion = false;
            bool flagInpos = false;
            EziMOTIONPlusELib.MOTION_OPTION_EX opt = new EziMOTIONPlusELib.MOTION_OPTION_EX();
            int lIncPos;
            uint lVelocity;

            Console.WriteLine("---------------------------");

            lIncPos = nDistance;
            lVelocity = 40000;
            opt.BIT_USE_CUSTOMACCEL = true;
            opt.BIT_USE_CUSTOMDECEL = true;
            opt.wCustomAccelTime = (ushort)nAccDecTime;
            opt.wCustomDecelTime = (ushort)nAccDecTime;

            Console.WriteLine("Move Motor! [Ex]");
            if (EziMOTIONPlusELib.FAS_MoveSingleAxisIncPosEx(nBdID, lIncPos, lVelocity, opt) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_MoveSingleAxisIncPosEx) was failed.");
                return false;
            }

            // Check the Axis status until motor stops and the Inposition value is checked
            do
            {
                Thread.Sleep(1);

                if (EziMOTIONPlusELib.FAS_GetAxisStatus(nBdID, ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetAxisStatus) was failed.");
                    return false;
                }

                flagMotion = (dwAxisStatus & 0x08000000 /*FFLAG_MOTIONING*/ ) != 0 ? true : false;
                flagInpos = (dwAxisStatus & 0x00080000 /*FFLAG_INPOSITION*/ ) != 0 ? true : false;

            } while (flagMotion || !flagInpos);

            return true;
        }
        #endregion

        #region 09.PositionOverrideExam
        static bool PosIncOverride(int nBdID)
        {
            // Move AxisIncPos & PositionIncOverride
            uint dwAxisStatus = 0;
            bool flagMotion = false;
            bool flagInpos = false;
            int lIncPos = INCPOS;
            uint lVelocity = 20000;
            int lActualPos = 0;
            int lChangePos = 0;
            int lPosDetect = 100000;

            Console.WriteLine("---------------------------");
            Console.WriteLine("[Inc Mode] Move Motor Start!");

            // 1. Move Command
            // Move the motor by INCPOS(200000) [pulse]
            if (EziMOTIONPlusELib.FAS_MoveSingleAxisIncPos(nBdID, lIncPos, lVelocity) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_MoveSingleAxisAbsPos) was failed.");
                return false;
            }

            // 2. Check Condition
            // Check current position
            do
            {
                Thread.Sleep(1);

                if (EziMOTIONPlusELib.FAS_GetActualPos(nBdID, ref lActualPos) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetActualPos) was failed.");
                    return false;
                }
            } while (lActualPos <= lPosDetect);

            // 3. Change Position
            // If the current position is less than the target position, change final position.
            lChangePos += lIncPos;
            if (EziMOTIONPlusELib.FAS_PositionIncOverride(nBdID, lChangePos) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_PositionIncOverride) was failed.");
                return false;
            }
            else
                Console.WriteLine("[Before] Target Position : {0}[pulse] / [After] Target Position : {1}[pulse]", lIncPos, lChangePos + lIncPos);

            // 4. Confirm Move Complete
            // Check the Axis status until motor stops and the Inposition value is checked
            do
            {
                Thread.Sleep(1);

                if (EziMOTIONPlusELib.FAS_GetAxisStatus(nBdID, ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetAxisStatus) was failed.");
                    return false;
                }

                flagMotion = (dwAxisStatus & 0x08000000 /*FFLAG_MOTIONING*/ ) != 0 ? true : false;
                flagInpos = (dwAxisStatus & 0x00080000 /*FFLAG_INPOSITION*/ ) != 0 ? true : false;

            } while (flagMotion || !flagInpos);
            return true;
        }

        static bool PosAbsOvrride(int nBdID)
        {
            // Move AxisAbsPos & PositionAbsOverride
            uint dwAxisStatus = 0;
            bool flagMotion = false;
            bool flagInpos = false;
            uint lVelocity = 40000;
            int lIncEndPos = 0;
            int lActualPos = 0;
            int lAbsPos;
            int lChangePos = 0;

            if (EziMOTIONPlusELib.FAS_GetActualPos(nBdID, ref lIncEndPos) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_GetActualPos) was failed.");
                return false;
            }

            Console.WriteLine("---------------------------");

            // 1. Move Command
            // Move the motor by ( (lIncEndPos)* 1/ 4) pulse (target position : Absolute position)
            lAbsPos = lIncEndPos / 4;
            if (EziMOTIONPlusELib.FAS_MoveSingleAxisAbsPos(nBdID, lAbsPos, lVelocity) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_MoveSingleAxisAbsPos) was failed.");
                return false;
            }
            Console.WriteLine("[ABS Mode] Move Motor Start!");

            // 2. Check Condition
            // Check current position
            do
            {
                Thread.Sleep(1);

                if (EziMOTIONPlusELib.FAS_GetActualPos(nBdID, ref lActualPos) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetActualPos) was failed.");
                    return false;
                }
            } while (lActualPos >= (lIncEndPos / 2));

            // 3. Change Position
            // if the current position falls below half the INC End position, change the target position to zero.
            lChangePos = ABSPOS;
            if (EziMOTIONPlusELib.FAS_PositionAbsOverride(nBdID, lChangePos) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_PositionAbsOverride) was failed.");
                return false;
            }
            else
                Console.WriteLine("Before Target Posistion : {0}[pulse] / Change Target Posistion : {1}[pulse]", lAbsPos, lChangePos);

            // 4. Confirm Move Complete
            // Check the Axis status until motor stops and the Inposition value is checked
            do
            {
                Thread.Sleep(1);

                if (EziMOTIONPlusELib.FAS_GetAxisStatus(nBdID, ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetAxisStatus) was failed.");
                    return false;
                }

                flagMotion = (dwAxisStatus & 0x08000000 /*FFLAG_MOTIONING*/ ) != 0 ? true : false;
                flagInpos = (dwAxisStatus & 0x00080000 /*FFLAG_INPOSITION*/ ) != 0 ? true : false;

            } while (flagMotion || !flagInpos);
            return true;
        }
        #endregion

        #region 10.VelocityOverrideExam
        static bool MovePosAndVelOverride(int nBdID)
        {
            // MoveSingleAxisIncPos & VelocityOverride

            // Move the motor by INCPOS(200000) [pulse]
            uint dwAxisStatus = 0;
            bool flagMotion = false;
            bool flagInpos = false;
            uint lVelocity = 20000;
            int lActualPos = 0;
            int lPosDetect = 100000;
            int lIncPos = INCPOS;
            uint lChangeVelocity = 0;

            Console.WriteLine("---------------------------");
            Console.WriteLine("[Inc Mode] Move Motor Start!");

            // 1. Move Command
            // Move the motor by INCPOS(200000) [pulse]
            if (EziMOTIONPlusELib.FAS_MoveSingleAxisIncPos(nBdID, lIncPos, lVelocity) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_MoveSingleAxisAbsPos) was failed.");
                return false;
            }

            // 2. Check Condition
            // Check current position
            do
            {
                Thread.Sleep(1);

                if (EziMOTIONPlusELib.FAS_GetActualPos(nBdID, ref lActualPos) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetActualPos) was failed.");
                    return false;
                }
            } while (lActualPos <= (lPosDetect));

            // 3. Change Velocity
            // If the current position is less than the target position, change velocity.
            lChangeVelocity = lVelocity * 2;
            if (EziMOTIONPlusELib.FAS_VelocityOverride(nBdID, lChangeVelocity) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_VelocityOverride) was failed.");
                return false;
            }
            else
                Console.WriteLine("Before Velocity : {0}[pps] / Change Velocity : {1}[pps]", lVelocity, lChangeVelocity);

            // 4. Confirm Move Complete
            // Check the Axis status until motor stops and the Inposition value is checked
            do
            {
                Thread.Sleep(1);

                if (EziMOTIONPlusELib.FAS_GetAxisStatus(nBdID, ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetAxisStatus) was failed.");
                    return false;
                }

                flagMotion = (dwAxisStatus & 0x08000000 /*FFLAG_MOTIONING*/ ) != 0 ? true : false;
                flagInpos = (dwAxisStatus & 0x00080000 /*FFLAG_INPOSITION*/ ) != 0 ? true : false;

            } while (flagMotion || !flagInpos);

            return true;
        }

        static bool JogAndVelOverride(int nBdID)
        {
            // Move Velocity & Velocity Override

            uint lVelocity = 10000;
            uint lChangeVelocity = 0;
            int lActualVelocity = 0;

            Console.WriteLine("---------------------------");
            Console.WriteLine("[Move Motor Start!]");

            // 1. Move Command
            // Move the motor 
            if (EziMOTIONPlusELib.FAS_MoveVelocity(nBdID, lVelocity, CW) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_MoveVelocity) was failed.");
                return false;
            }
            else
                Console.WriteLine("Current Velocity : {0}[pps] ", lVelocity);

            Console.WriteLine(">> Wait... 3 Second");
            Thread.Sleep(3 * 1000);

            // 2. Change Velocity

            if (EziMOTIONPlusELib.FAS_GetActualVel(nBdID, ref lActualVelocity) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_GetActualVel) was failed.");
                return false;
            }

            lChangeVelocity = (uint)lActualVelocity * 2;
            if (EziMOTIONPlusELib.FAS_VelocityOverride(nBdID, lChangeVelocity) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_VelocityOverride) was failed.");
                return false;
            }
            else
                Console.WriteLine("Before Velocity : {0}[pps] / Change Velocity : {1}[pps]", lActualVelocity, lChangeVelocity);

            Console.WriteLine(">> Wait... 5 Second");
            Thread.Sleep(5 * 1000);

            // 3. Change Velocity
            if (EziMOTIONPlusELib.FAS_GetActualVel(nBdID, ref lActualVelocity) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_VelocityOverride) was failed.");
                return false;
            }

            lChangeVelocity = (uint)lActualVelocity * 2;
            if (EziMOTIONPlusELib.FAS_VelocityOverride(nBdID, lChangeVelocity) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_VelocityOverride) was failed.");
                return false;
            }
            else
                Console.WriteLine("Before Velocity : {0}[pps] / Change Velocity : {1}[pps]", lActualVelocity, lChangeVelocity);

            Console.WriteLine(">> Wait... 10 Second");
            Thread.Sleep(10 * 1000);

            // 4. Stop
            EziMOTIONPlusELib.FAS_MoveStop(nBdID);
            Console.WriteLine("FAS_MoveStop!");

            return true;
        }

        public static void VelocityOverride(int nBdID, int lChangeVelocity)
        {
            if (EziMOTIONPlusELib.FAS_VelocityOverride(nBdID, (uint)lChangeVelocity) != EziMOTIONPlusELib.FMM_OK)
            {
                //Console.WriteLine("Function(FAS_VelocityOverride) was failed.");
                //return false;
            }
        }
        #endregion

        #region 11.OriginSearchExam
        static bool SetOriginParameter(int nBdID)
        {
            // Set Origin Parameter
            int nOrgSpeed = 50000;
            int nOrgSearchSpeed = 1000;
            int nOrgAccDecTime = 50;
            int nOrgMethod = 2; // Origin Method = 2 is 'Limit Origin' in the Ezi-SERVOII Plus-E model.
            int nOrgDir = CW;
            int nOrgOffset = 0;
            int nOrgPositionSet = 0;
            int nOrgTorqueRatio = 50;

            if (EziMOTIONPlusELib.FAS_SetParameter(nBdID, 14 /*SERVO2_ORGSPEED*/, nOrgSpeed) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetParameter[SERVO2_ORGSPEED]) was failed.");
                return false;
            }

            if (EziMOTIONPlusELib.FAS_SetParameter(nBdID, 15 /*SERVO2_ORGSEARCHSPEED*/, nOrgSearchSpeed) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetParameter[SERVO2_ORGSEARCHSPEED]) was failed.");
                return false;
            }

            if (EziMOTIONPlusELib.FAS_SetParameter(nBdID, 16 /*SERVO2_ORGACCDECTIME*/, nOrgAccDecTime) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetParameter[SERVO2_ORGACCDECTIME]) was failed.");
                return false;
            }

            if (EziMOTIONPlusELib.FAS_SetParameter(nBdID, 17 /*SERVO2_ORGMETHOD*/, nOrgMethod) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetParameter[SERVO2_ORGMETHOD]) was failed.");
                return false;
            }

            if (EziMOTIONPlusELib.FAS_SetParameter(nBdID, 18 /*SERVO2_ORGDIR*/, nOrgDir) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetParameter[SERVO2_ORGDIR]) was failed.");
                return false;
            }

            if (EziMOTIONPlusELib.FAS_SetParameter(nBdID, 19 /*SERVO2_ORGOFFSET*/, nOrgOffset) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetParameter[SERVO2_ORGOFFSET]) was failed.");
                return false;
            }

            if (EziMOTIONPlusELib.FAS_SetParameter(nBdID, 20 /*SERVO2_ORGPOSITIONSET*/, nOrgPositionSet) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetParameter[SERVO2_ORGPOSITIONSET]) was failed.");
                return false;
            }

            if (EziMOTIONPlusELib.FAS_SetParameter(nBdID, 27 /*SERVO2_ORGTORQUERATIO*/, nOrgTorqueRatio) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetParameter[SERVO2_ORGTORQUERATIO]) was failed.");
                return false;
            }

            return true;
        }

        public static bool GetOriginOffset(int nBdID, out int nOrgOffset)
        {
            //if (EziMOTIONPlusELib.FAS_SetParameter(nBdID, 19 /*SERVO2_ORGOFFSET*/, nOrgOffset) != EziMOTIONPlusELib.FMM_OK)
            //{
            //    Console.WriteLine("Function(FAS_SetParameter[SERVO2_ORGOFFSET]) was failed.");
            //    return false;
            //}         
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            nOrgOffset = 0;
            int lParamVal = 0;
            int nRtn;
            // Check The Axis Start Speed Parameter Status
            nRtn = EziMOTIONPlusELib.FAS_GetParameter(nBdID, 19 /*SERVO2_ORGOFFSET*/, ref lParamVal);
            if (nRtn != EziMOTIONPlusELib.FMM_OK)
            {
                //Console.WriteLine($"[{methodName}] ::: Function(FAS_GetParameter) was failed.");
                return false;
            }
            else
            {
                //Console.WriteLine("Load Parameter[Before] : Start Speed = {0}[pps]", lParamVal);
            }
            nOrgOffset = lParamVal;
            return true;
        }

        public static bool OriginSearch(int nBdID)
        {
            // Act Origin Search Function
            //uint dwAxisStatus = 0;
            //bool flagOriginRtn = false;
            //bool flagOriginOk = false;

            // Set Origin Parameter
            int nOrgSpeed = 20000;//10mmsec
            int nOrgSearchSpeed = 1000;
            int nOrgAccDecTime = 50;
            //int nOrgMethod = 2; // Origin Method = 2 is 'Limit Origin' in the Ezi-SERVOII Plus-E model.
            //int nOrgDir = CW;
            //int nOrgOffset = 0;
            //int nOrgPositionSet = 0;
            //int nOrgTorqueRatio = 50;

            //Console.WriteLine("---------------------------");

            //2023.06.14 
            if (Define.eCUSTOM == ECUSTOM.Demo)
            {
                nOrgSpeed = 50000;//10mmsec
            }

            if (EziMOTIONPlusELib.FAS_SetParameter(nBdID, 14 /*SERVO2_ORGSPEED*/, nOrgSpeed) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetParameter[SERVO2_ORGSPEED]) was failed.");
                return false;
            }

            if (EziMOTIONPlusELib.FAS_SetParameter(nBdID, 15 /*SERVO2_ORGSEARCHSPEED*/, nOrgSearchSpeed) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetParameter[SERVO2_ORGSEARCHSPEED]) was failed.");
                return false;
            }

            if (EziMOTIONPlusELib.FAS_SetParameter(nBdID, 16 /*SERVO2_ORGACCDECTIME*/, nOrgAccDecTime) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetParameter[SERVO2_ORGACCDECTIME]) was failed.");
                return false;
            }

            // Origin Search Start
            if (EziMOTIONPlusELib.FAS_MoveOriginSingleAxis(nBdID) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_MoveOriginSingleAxis) was failed.");
                return false;
            }

            // Check the Axis status until OriginReturning value is released.
            //do
            //{
            //    Thread.Sleep(1);
            //
            //    if (EziMOTIONPlusELib.FAS_GetAxisStatus(nBdID, ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
            //    {
            //        Console.WriteLine("Function(FAS_GetAxisStatus) was failed.");
            //        return false;
            //    }
            //
            //    flagOriginRtn = (dwAxisStatus & 0x00040000 /*FFLAG_ORIGINRETURNING*/ ) != 0 ? true : false;
            //} while (flagOriginRtn);
            //
            //flagOriginOk = (dwAxisStatus & 0x02000000 /*FFLAG_ORIGINRETOK*/ ) != 0 ? true : false;
            //if (flagOriginOk)
            //{
            //    Console.WriteLine("Origin Search Success!");
            //    return true;
            //}
            //else
            //{
            //    Console.WriteLine("Origin Search Fail !");
            //    return false;
            //}
            return true;
        }
        #endregion

        #region 12.SetPositionTableExam
        static bool SetPosTable(int nBdID)
        {
            // Sets values in the position table.
            EziMOTIONPlusELib.ITEM_NODE nodeItem = new EziMOTIONPlusELib.ITEM_NODE();
            ushort wItemNo = 1;

            Console.WriteLine("---------------------------");
            // Gets the data values of that node.
            if (EziMOTIONPlusELib.FAS_PosTableReadItem(nBdID, wItemNo, ref nodeItem) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_PosTableReadItem) was failed.");
                return false;
            }

            // Overwrite part of node's data
            nodeItem.dwMoveSpd = 50000;
            nodeItem.lPosition = 250000;
            nodeItem.wBranch = 3;
            nodeItem.wContinuous = 1;

            // Set node data in Position Table
            if (EziMOTIONPlusELib.FAS_PosTableWriteItem(nBdID, wItemNo, nodeItem) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_PosTableWriteItem) was failed.");
                return false;
            }
            else
                Console.WriteLine("Setting PosTable Data !");

            return true;
        }

        static bool RunPosTable(int nBdID)
        {
            uint dwAxisStatus = 0;
            bool flagPtStop = false;
            ushort wItemNo = 1;

            Console.WriteLine("---------------------------");
            if (EziMOTIONPlusELib.FAS_PosTableRunItem(nBdID, wItemNo) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_PosTableRunItem) was failed.");
                return false;
            }

            Console.WriteLine("PosTable Run!");

            do
            {
                Thread.Sleep(1);

                if (EziMOTIONPlusELib.FAS_GetAxisStatus(nBdID, ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetAxisStatus) was failed.");
                    return false;
                }

                flagPtStop = (dwAxisStatus & 0x00400000 /*FFLAG_PTSTOPPED*/ ) != 0 ? true : false;
                if (flagPtStop == true)
                    Console.WriteLine("Position Table Run Stop!");
            } while (!flagPtStop);

            return true;
        }
        #endregion

        #region 13.PushStopExam
        static bool OperatePushMode(int nBdID)
        {
            uint dwAxisStatus = 0;
            bool flagMotion = false;
            uint dwStartSpd, dwMoveSpd;
            ushort wAccel, wDecel;
            int lPosition;

            uint dwPushSpd;
            ushort wPushRate;
            ushort wPushMode;
            int lEndPosition;

            // normal position motion
            dwStartSpd = 1;
            dwMoveSpd = 50000;
            wAccel = 500;
            wDecel = 500;
            lPosition = 500000;

            // push motion
            dwPushSpd = 2000;
            wPushRate = 50;
            wPushMode = 0;  // Stop Mode Push
            //wPushMode = 100;	// Non-stop Mode Push & 100 pulse draw-back
            lEndPosition = lPosition + 10000;

            Console.WriteLine("---------------------------");

            if (EziMOTIONPlusELib.FAS_MovePush(nBdID, dwStartSpd, dwMoveSpd, lPosition, wAccel, wDecel, wPushRate, dwPushSpd, lEndPosition, wPushMode) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_MovePush) was failed.");
                return false;
            }

            // Check the Axis status until motor stops and the Inposition value is checked
            do
            {
                Thread.Sleep(1);

                if (EziMOTIONPlusELib.FAS_GetAxisStatus(nBdID, ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetAxisStatus) was failed.");
                    return false;
                }

                flagMotion = (dwAxisStatus & 0x08000000 /*FFLAG_MOTIONING*/ ) != 0 ? true : false;
            } while (flagMotion);

            return true;
        }

        static bool CheckPushResult(int nBdID)
        {
            uint dwOutput = 0;
            bool flagWork = false;

            Console.WriteLine("---------------------------");
            Console.WriteLine("Check Result Function");

            if (EziMOTIONPlusELib.FAS_GetIOOutput(nBdID, ref dwOutput) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_GetIOOutput) was failed.");
                return false;
            }
            else
            {
                flagWork = (dwOutput & 0x00000080 /*BIT_RESERVED0*/ ) != 0 ? true : false;

                if (flagWork == true)
                    Console.WriteLine("Work Detected!");
                else
                    Console.WriteLine("Work Not Detected!");
            }

            return true;
        }

        static bool ReturnPosition(int nBdID)
        {
            // Return Position
            uint dwAxisStatus = 0;
            bool flagMotion = false;
            bool flagInpos = false;
            int lAbsPos;
            uint lVelocity;

            // Move the motor by 0 pulse (target position : Absolute position)
            lAbsPos = 0;
            lVelocity = 50000;

            Console.WriteLine("---------------------------");
            Console.WriteLine("Return Postion Start!");
            if (EziMOTIONPlusELib.FAS_MoveSingleAxisAbsPos(nBdID, lAbsPos, lVelocity) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_MoveSingleAxisAbsPos) was failed.");
                return false;
            }

            // Check the Axis status until motor stops and the Inposition value is checked
            do
            {
                Thread.Sleep(1);

                if (EziMOTIONPlusELib.FAS_GetAxisStatus(nBdID, ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetAxisStatus) was failed.");
                    return false;
                }

                flagMotion = (dwAxisStatus & 0x08000000 /*FFLAG_MOTIONING*/ ) != 0 ? true : false;
                flagInpos = (dwAxisStatus & 0x00080000 /*FFLAG_INPOSITION*/ ) != 0 ? true : false;

            } while (flagMotion || !flagInpos);

            return true;
        }
        #endregion

        #region 14.PushNonStopExam
        static bool PrintStatus(int nBdID)
        {
            int lActPos = 0;
            uint dwOutput = 0;
            bool flagWork = false;
            DateTime endTime;

            // Check the Axis status while 10 Seconds.
            endTime = DateTime.Now.AddSeconds(10);
            do
            {
                Thread.Sleep(1);

                if (EziMOTIONPlusELib.FAS_GetActualPos(nBdID, ref lActPos) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetActualPos) was failed.");
                    return false;
                }

                if (EziMOTIONPlusELib.FAS_GetIOOutput(nBdID, ref dwOutput) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetIOOutput) was failed.");
                    return false;
                }

                flagWork = (dwOutput & 0x00000080 /*BIT_RESERVED0*/ ) != 0 ? true : false;

                Console.WriteLine("Position {0} : {1}", lActPos, (flagWork ? "Work Detected!" : "Work Not detected"));

            } while (DateTime.Now < endTime);

            return true;
        }

        public static bool StopMotion(int nBdID, bool bEmergency)
        {
            // Move Stop And Check Motion
            uint dwAxisStatus = 0;
            bool flagMotion = false;


            if (bEmergency)
            {
                if (EziMOTIONPlusELib.FAS_EmergencyStop(nBdID) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_EmergencyStop) was failed.");
                }
                else
                {
                    Console.WriteLine("Move Stop!");
                }
            }
            else
            {
                if (EziMOTIONPlusELib.FAS_MoveStop(nBdID) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_MoveStop) was failed.");
                }
                else
                {
                    Console.WriteLine("Move Stop!");
                }
            }

            

            do
            {
                Thread.Sleep(1);

                if (EziMOTIONPlusELib.FAS_GetAxisStatus(nBdID, ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetAxisStatus) was failed.");
                    return false;
                }

                flagMotion = (dwAxisStatus & 0x08000000 /*FFLAG_MOTIONING*/ ) != 0 ? true : false;
            } while (flagMotion);

            return true;
        }
        #endregion

        #region 15.GetInputExam
        public static bool GetInput(int nBdID, out uint dat)
        {
            uint uInput = 0;
            uint uLatch = 0;
            dat = 0;
            if (EziMOTIONPlusELib.FAS_GetInput(nBdID, ref uInput, ref uLatch) != EziMOTIONPlusELib.FMM_OK)
            {
                return false;
            }
            dat = uInput;
            return true;
        }
        #endregion

        #region 16.LatchExam
        static bool LatchCount(int nBdID)
        {
            uint uInput = 0;
            uint uLatch = 0;

            byte cInputNo = 0;  // Pin 0
            uint nLatchCount = 0;

            uint[] cntLatchAll = new uint[16];
            uint uLatchMask;

            Console.WriteLine("Monitor a specific pin Input latch signal while 1 min...");

            // Monitor the specific pin input result value.
            do
            {
                // Latch status
                if (EziMOTIONPlusELib.FAS_GetInput(nBdID, ref uInput, ref uLatch) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetInput) was failed.");
                    return false;
                }

                // Latch count
                if (EziMOTIONPlusELib.FAS_GetLatchCount(nBdID, cInputNo, ref nLatchCount) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetLatchCount) was failed.");
                    return false;
                }

                Console.WriteLine("Pin %d is %s and %s (latch count %d)", cInputNo, (((uInput & (0x01 << cInputNo)) != 0) ? "ON" : "OFF"), (((uLatch & (0x01 << cInputNo)) != 0) ? "latched" : "not latched"), nLatchCount);

            } while (WaitSeconds(60));

            // Clear the specific pin's Latch status
            uLatchMask = (uint)(0x01 << cInputNo);
            if (EziMOTIONPlusELib.FAS_ClearLatch(nBdID, uLatchMask) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_ClearLatch) was failed.");
                return false;
            }
            else
            {
                Console.WriteLine("FAS_ClearLatch Success!");
            }

            // Get Latch status again
            if (EziMOTIONPlusELib.FAS_GetInput(nBdID, ref uInput, ref uLatch) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_GetInput) was failed.");
                return false;
            }

            Console.WriteLine("Pin %d is %s", cInputNo, (((uLatch & (0x01 << cInputNo)) != 0) ? "latched" : "not latched"));

            // Get latch counts of all inputs (16 inputs)
            if (EziMOTIONPlusELib.FAS_GetLatchCountAll(nBdID, cntLatchAll) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_GetLatchCountAll) was failed.");
                return false;
            }
            else
            {
                for (int i = 0; i < 16; i++)
                {
                    Console.WriteLine("[FAS_GetLatchCountAll]  Pin[%d] : [%d]count", i, cntLatchAll[i]);
                }
            }

            // Clear the latch count of the specific pin
            if (EziMOTIONPlusELib.FAS_ClearLatchCount(nBdID, cInputNo) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_ClearLatchCount) was failed.");
                return false;
            }
            else
            {
                Console.WriteLine("FAS_ClearLatchCount Success!");
            }

            return true;
        }
        #endregion

        #region 17.SetOutputExam

        public static bool GetOutput(int nBdID, out uint dat)
        {
            uint uOutput = 0;
            uint uStatus = 0;        
            dat = 0;
            if (EziMOTIONPlusELib.FAS_GetOutput(nBdID, ref uOutput, ref uStatus) != EziMOTIONPlusELib.FMM_OK)
            {
                return false;
            }
            dat = uOutput;
            return true;
        }
        static bool SetOutput(int nBdID)
        {
            uint uOutput = 0;
            uint uStatus = 0;

            uint uSetMask;
            uint uClrMask;
            bool bON;

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Clearing All outputs (bit 0 ~ bit 15.");
            uSetMask = 0x00;
            uClrMask = 0x00FF;
            if (EziMOTIONPlusELib.FAS_SetOutput(nBdID, uSetMask, uClrMask) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetOutput) was failed.");
            }
            else
            {
                Console.WriteLine("FAS_SetOutput Success!");
            }

            // Check OutputPin Status
            if (EziMOTIONPlusELib.FAS_GetOutput(nBdID, ref uOutput, ref uStatus) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_GetOutput) was failed.");
            }
            else
            {
                for (int i = 0; i < 16; i++)
                {
                    bON = ((uOutput & (0x01 << i)) != 0);
                    Console.WriteLine("OutPin[{0}] = {1}", i, ((bON) ? "ON" : "OFF"));
                }
            }

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Set output bit #0.");
            uSetMask = 0x01;
            uClrMask = 0x00;
            if (EziMOTIONPlusELib.FAS_SetOutput(nBdID, uSetMask, uClrMask) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetOutput) was failed.");
            }
            else
            {
                Console.WriteLine("FAS_SetOutput Success!");
            }

            // Check OutputPin Status
            if (EziMOTIONPlusELib.FAS_GetOutput(nBdID, ref uOutput, ref uStatus) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_GetOutput) was failed.");
            }
            else
            {
                for (int i = 0; i < 16; i++)
                {
                    bON = ((uOutput & (0x01 << i)) != 0);
                    Console.WriteLine("OutPin[{0}] = {1}", i, ((bON) ? "ON" : "OFF"));
                }
            }

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Clear bit #0 and Set bit #3, #4, #5.");
            uSetMask = 0x08 | 0x10 | 0x20;
            uClrMask = 0x01;
            if (EziMOTIONPlusELib.FAS_SetOutput(nBdID, uSetMask, uClrMask) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetOutput) was failed.");
            }
            else
            {
                Console.WriteLine("FAS_SetOutput Success!");
            }

            // Check OutputPin Status
            if (EziMOTIONPlusELib.FAS_GetOutput(nBdID, ref uOutput, ref uStatus) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_GetOutput) was failed.");
            }
            else
            {
                for (int i = 0; i < 16; i++)
                {
                    bON = ((uOutput & (0x01 << i)) != 0);
                    Console.WriteLine("OutPin[{0}] = {1}", i, ((bON) ? "ON" : "OFF"));
                }
            }

            return true;
        }

        public static bool SetOutput(int nBdID, int OnOff, uint nBitData)
        {
            uint uSetMask = 0;
            uint uClrMask = 0;

            if (OnOff == 1)
            {
                uSetMask = nBitData << 16;
            }
            else if (OnOff == 0)
            {
                uClrMask = nBitData << 16;
            }

            //Console.WriteLine($"uSetMask ::: 0x{uSetMask:X4} uClrMask ::: 0x{uClrMask:X4}");

            if (EziMOTIONPlusELib.FAS_SetOutput(nBdID, uSetMask, uClrMask) != EziMOTIONPlusELib.FMM_OK)
            {   
                return false;
            }
            return true;
        }

        public static bool SetOutput(int nBdID, bool[] setData)
        {
            if (setData.Length != 16)
                return false;

            uint uSetMask = 0;
            uint uClrMask = 0;

            for (int i = 0; i < 16; i++)
            {
                if (setData[i] == true)
                {
                    uSetMask |= (uint)(1 << i);
                }
                if(setData[i] == false)
                {
                    uClrMask |= (uint)(1 << i);
                }

            }

            uSetMask = uSetMask << 16;
            uClrMask = uClrMask << 16;

            //Console.WriteLine($"??? uSetMask ::: 0x{uSetMask:X4} uClrMask ::: 0x{uClrMask:X4}");

            if (EziMOTIONPlusELib.FAS_SetOutput(nBdID, uSetMask, uClrMask) != EziMOTIONPlusELib.FMM_OK)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 18.TriggerExam
        static bool SetTrigger(int nBdID)
        {
            EziMOTIONPlusELib.TRIGGER_INFO Trg_Info = new EziMOTIONPlusELib.TRIGGER_INFO();
            byte cOutputNo;

            uint uRunMask;
            uint uStopMask;
            uint uCount = 0;

            // Set Trigger value.
            cOutputNo = 0;          // Output pin #0
            Trg_Info.wCount = 20;   // Set the number of Trigger outputs
            Trg_Info.wOnTime = 250; // On Time Setting : 250 [ms]
            Trg_Info.wPeriod = 500; // Set Trigger period : 500 [ms]

            if (EziMOTIONPlusELib.FAS_SetTrigger(nBdID, cOutputNo, Trg_Info) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetTrigger) was failed.");
                return false;
            }
            else
            {
                Console.WriteLine("FAS_SetTrigger Success!");
            }

            // Run Output
            uRunMask = (uint)(0x01 << cOutputNo); // Run Output Pin #0
            uStopMask = 0x00000000;
            if (EziMOTIONPlusELib.FAS_SetRunStop(nBdID, uRunMask, uStopMask) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetRunStop) was failed.");
                return false;
            }
            else
            {
                Console.WriteLine("FAS_SetRunStop Success!");
            }

            do
            {
                Thread.Sleep(100);

                if (EziMOTIONPlusELib.FAS_GetTriggerCount(nBdID, cOutputNo, ref uCount) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetTriggerCount) was failed.");
                    return false;
                }
                else
                {
                    Console.WriteLine("Get Trigger [{0}] count", uCount);
                }
            } while (uCount <= Trg_Info.wCount);

            return true;
        }
        #endregion

        #region 19.IOLevelExam
        static bool GetIOLevel(int nBdID)
        {
            uint uIOLevel = 0;
            bool bLevel;

            Console.WriteLine("----------------------------------");
            // [Before] Check IO Level Status
            if (EziMOTIONPlusELib.FAS_GetIOLevel(nBdID, ref uIOLevel) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_GetIOLevel) was failed.");
                return false;
            }
            Console.WriteLine("Load IO Level Status : 0x%08x", uIOLevel);

            for (int i = 0; i < 16; i++)
            {
                bLevel = ((uIOLevel & (0x01 << i)) != 0);
                Console.WriteLine("I/O pin %d : %s", i, (bLevel) ? "High Active" : "Low Active");
            }

            return true;
        }

        static bool SetIOLevel(int nBdID)
        {
            uint uIOLevel = 0x0000ff00;

            Console.WriteLine("----------------------------------");
            // Set IO Level Status
            if (EziMOTIONPlusELib.FAS_SetIOLevel(nBdID, uIOLevel) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_SetIOLevel) was failed.");
            }
            else
            {
                Console.WriteLine("Set IO Level Status : 0x%08x", uIOLevel);
            }

            return true;
        }
        #endregion

        #region 20.MoveLinearAbsIncPos
        static bool MoveLinearIncPos(int[] nBdID)
        {
            // Move Linear IncPos
            bool flagMotion = false;
            bool flagInpos = false;
            int[] lIncPos;
            uint lVelocity = 0;
            uint dwAxisStatus = 0;
            ushort wAcceltime = 100;

            Console.WriteLine("---------------------------");
            // Increase the motor by 370000 pulse (target position : Relative position)

            lIncPos = new int[] { 370000, 370000 };
            lVelocity = 40000;

            Console.WriteLine("[Linear Inc Mode] Move Motor !");

            if (EziMOTIONPlusELib.FAS_MoveLinearIncPos2(SLAVE_CNT, nBdID, lIncPos, lVelocity, wAcceltime) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_MoveLinearIncPos2) was failed.");
                return false;
            }

            // Check the Axis status until motor stops and the Inposition value is checked
            for (int nID = 0; nID < SLAVE_CNT; nID++)
            {
                do
                {
                    Thread.Sleep(1);

                    if (EziMOTIONPlusELib.FAS_GetAxisStatus(nBdID[nID], ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
                    {
                        Console.WriteLine("[nBdID : {0}] Function(FAS_GetAxisStatus) was failed.", nBdID[nID]);
                        return false;
                    }

                    flagMotion = (dwAxisStatus & 0x08000000 /*FFLAG_MOTIONING*/) != 0 ? true : false;
                    flagInpos = (dwAxisStatus & 0x00080000 /*FFLAG_INPOSITION*/) != 0 ? true : false;

                } while (flagMotion || !flagInpos);
            }


            return true;
        }

        static bool MoveLinearAbsPos(int[] nBdID)
        {
            // Move LinearAbsPos 
            uint dwAxisStatus = 0;
            bool flagMotion = false;
            bool flagInpos = false;
            int[] lAbsPos;
            uint lVelocity = 0;
            ushort wAcceltime = 100;

            Console.WriteLine("---------------------------");

            // Move the motor by 0 pulse (target position : Absolute position)
            lAbsPos = new int[] { 0, 0 };
            lVelocity = 40000;

            Console.WriteLine("[Linear Abs Mode] Move Motor!");
            if (EziMOTIONPlusELib.FAS_MoveLinearAbsPos2(SLAVE_CNT, nBdID, lAbsPos, lVelocity, wAcceltime) != EziMOTIONPlusELib.FMM_OK)
            {
                Console.WriteLine("Function(FAS_MoveLinearAbsPos2) was failed.");
                return false;
            }

            // Check the Axis status until motor stops and the Inposition value is checked
            for (int nID = 0; nID < SLAVE_CNT; nID++)
            {
                do
                {
                    Thread.Sleep(1);

                    if (EziMOTIONPlusELib.FAS_GetAxisStatus(nBdID[nID], ref dwAxisStatus) != EziMOTIONPlusELib.FMM_OK)
                    {
                        Console.WriteLine("[nBdID : {0}] Function(FAS_GetAxisStatus) was failed.", nBdID[nID]);
                        return false;
                    }

                    flagMotion = (dwAxisStatus & 0x08000000 /*FFLAG_MOTIONING*/) != 0 ? true : false;
                    flagInpos = (dwAxisStatus & 0x00080000 /*FFLAG_INPOSITION*/) != 0 ? true : false;

                } while (flagMotion || !flagInpos);
            }

            return true;
        }
        #endregion

        public static bool SetReConnect(int nBdID, bool IsConnected)
        {
            int nRtn;
            Console.WriteLine("-----------------------------------------------------------------------------");
            if (IsConnected == false)
            {
                nRtn = EziMOTIONPlusELib.FAS_Reconnect(nBdID);
                if (nRtn != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_Reconnect) was failed.");
                    return false;
                }
                else
                {
                    //Console.WriteLine("");
                }
            }            
            //nRtn = EziMOTIONPlusELib.FAS_SetAutoReconnect(1);
            //if (nRtn != EziMOTIONPlusELib.FMM_OK)
            //{
            //    Console.WriteLine("Function(FAS_SetAutoReconnect) was failed.");
            //    return false;
            //}
            //else
            //{
            //    //Console.WriteLine("");
            //}
            Console.WriteLine("-----------------------------------------------------------------------------");


            return true;
        }

        public static bool IsConnLost(int nBdID, string ipAddress)
        {
            int nRtn;
            bool IsBoardLost = false;
            bool IsIpaddrLost = false;
            bool IsConnect = false;
            IPAddress ip = IPAddress.Parse(ipAddress);
            IsBoardLost = EziMOTIONPlusELib.FAS_IsBdIDExist(nBdID, ref ip);
            IsIpaddrLost = EziMOTIONPlusELib.FAS_IsIPAddressExist(ip, ref nBdID);
            nRtn = EziMOTIONPlusELib.FAS_IsSlaveExist(nBdID);
            if (nRtn != EziMOTIONPlusELib.FMM_OK)
            {
                //Console.WriteLine($"Function(FAS_IsSlaveExist) ::: {nRtn}");
            }
            else
            {
                //return SetReConnect(nBdID, false);
                EziMOTIONPlusELib.FAS_Close(nBdID);
                IsConnect = Connect(ipAddress, nBdID);
                //Console.WriteLine($"Function(IsConnLost) ::: {IsConnect}");
            }

            return true;
        }
    }
}
