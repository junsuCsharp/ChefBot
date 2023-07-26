using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using PackML;

namespace Cores
{
    public class Core_Adapter
    {
        //2022.12.29 ::: 하위 수정 금지 Core_Program PackML 프로세스와 같이 변경 해야 합니다.
        const int CurrentState_None = 00;
        const int CurrentState_Starting = 02;
        const int CurrentState_Completing = 04;
        const int CurrentState_Resetting = 06;
        const int CurrentState_Holding = 07;
        const int CurrentState_Unholding = 09;
        const int CurrentState_Suspending = 10;
        const int CurrentState_Suspended = 11;
        const int CurrentState_Stopping = 13;
        const int CurrentState_Aborting = 15;
        const int CurrentState_Clearing = 17;

        const int On = 1;
        const int Off = 0;

        public const int Edge_High = 1;
        public const int Edge_Low = 0;

        public event ProcessReachedEventHandler OnProcessCommand;
        public delegate void ProcessReachedEventHandler(object sender, ProcessReachedEventArgs e);

        private Dictionary<string, AcceesInput> dicInput = new Dictionary<string, AcceesInput>();

        public void SetPunches(uint CurrInput, uint msecDelay, uint Activity, int cmd)
        {

            //Internal ::: Var Name, timeSpan
            string name = CommandName(cmd);
            AcceesInput dcc = new AcceesInput();
            ProcessReachedEventArgs args = new ProcessReachedEventArgs();
            if (dicInput.TryGetValue(name, out AcceesInput acc))
            {
                if (CurrInput != acc.prevInput && msecDelay == Off && CurrInput == Activity)
                {   
                    args.Command = cmd;
                    args.TimeReached = DateTime.Now;
                    OnProcessCommandReached(args);
                }
                else if (CurrInput != acc.prevInput && msecDelay > Off && CurrInput == Activity)
                {
                    dcc.IsTimerStart = true;                    
                    dcc.ts = new TimeSpan(0, 0, 0, 0, (int)msecDelay);
                    dcc.dt = DateTime.Now.Add(dcc.ts);

                    //Console.WriteLine($"Start {DateTime.Now} / {CurrInput}");
                }
                else
                {
                    //Timer                    
                    if (DateTime.Now  >= acc.dt && acc.IsTimerStart)
                    {
                        if (CurrInput == Activity)
                        {
                            //acc
                            //Console.WriteLine($"End {acc.dt} / {DateTime.Now} / {CurrInput}");
                            args.Command = cmd;
                            args.TimeReached = DateTime.Now;
                            OnProcessCommandReached(args);
                        }
                        acc.IsTimerStart = false;
                    }
                }

                //전데이터 비교
                if (acc.IsTimerStart == true && dcc.IsTimerStart == false)
                {
                    dcc.IsTimerStart = true;
                    dcc.ts = new TimeSpan(0, 0, 0, 0, (int)msecDelay);
                    dcc.dt = acc.dt;
                }             

                dicInput.Remove(name);
            }           

            dcc.name = name;
            dcc.msec = msecDelay;
            dcc.activity = Activity;

            dcc.prevInput = CurrInput;
            dicInput.Add(name, dcc);
        }

        private string CommandName(int command)
        {
            string name = null;
            switch (command)
            {
                case CurrentState_None:
                    name = nameof(CurrentState_None);
                    break;
                case CurrentState_Starting:
                    name = nameof(CurrentState_Starting);
                    break;
                case CurrentState_Completing:
                    name = nameof(CurrentState_Completing);
                    break;
                case CurrentState_Resetting:
                    name = nameof(CurrentState_Resetting);
                    break;
                case CurrentState_Holding:
                    name = nameof(CurrentState_Holding);
                    break;
                case CurrentState_Unholding:
                    name = nameof(CurrentState_Unholding);
                    break;
                case CurrentState_Suspending:
                    name = nameof(CurrentState_Suspending);
                    break;
                case CurrentState_Suspended:
                    name = nameof(CurrentState_Suspended);
                    break;
                case CurrentState_Stopping:
                    name = nameof(CurrentState_Stopping);
                    break;
                case CurrentState_Aborting:
                    name = nameof(CurrentState_Aborting);
                    break;
                case CurrentState_Clearing:
                    name = nameof(CurrentState_Clearing);
                    break;
                default:
                    name = "Unknown";
                    break;
            }

            return name;
        }

        private bool Push_up(uint CurrInput, uint PrevInput, float secTime, uint Activity)
        {
            bool boolRtn = false;




            return boolRtn;
        }


        public class AcceesInput
        {
            public string name;
            public TimeSpan ts;
            public DateTime dt;
            public uint msec;
            public uint prevInput;
            public uint activity;
            public bool IsTimerStart = false;
        }


        protected virtual void OnProcessCommandReached(ProcessReachedEventArgs e)
        {
            ProcessReachedEventHandler handler = OnProcessCommand;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public class ProcessReachedEventArgs : EventArgs
        {
            public int Command { get; set; }
            public DateTime TimeReached { get; set; }

            //public EStateCommand eStateCommand { get; set; }
        }

    }

   
}
