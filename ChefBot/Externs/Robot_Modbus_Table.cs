using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace Externs
{
    public class Robot_Modbus_Table
    {
        public static string strRobot_Version = null;

        public static List<Data> lstModbusData = new List<Data>() {
            new Data(000, Type.Read , Func.Holding_Register, "ControlBox Digital Input(1~16)     ", new Point(01, 1), "0b1111 1111 1111 1111   							", true),
            new Data(001, Type.Both , Func.Holding_Register, "ControlBox Digital Output(1~16)    ", new Point(01, 1), "0b1111 1111 1111 1111   							", true),
            new Data(004, Type.Read , Func.Holding_Register, "ControlBox Analog Input 1          ", new Point(01, 1), "float                   							", false),
            new Data(005, Type.Both , Func.Holding_Register, "ControlBox Analog Input 1 Type     ", new Point(01, 1), "0 : Current, 1 : Voltage							", false),
            new Data(006, Type.Read , Func.Holding_Register, "ControlBox Analog Input 2          ", new Point(01, 1), "float                   							", false),
            new Data(007, Type.Both , Func.Holding_Register, "ControlBox Analog Input 2 Type     ", new Point(01, 1), "0 : Current, 1 : Voltage							", false),
            new Data(016, Type.Write, Func.Holding_Register, "ControlBox Analog Output 1         ", new Point(01, 1), "float                   							", false),
            new Data(017, Type.Write, Func.Holding_Register, "ControlBox Analog Output 1 Type    ", new Point(01, 1), "0 : Current, 1 : Voltage							", false),
            new Data(018, Type.Write, Func.Holding_Register, "ControlBox Analog Output 2         ", new Point(01, 1), "float                   							", false),
            new Data(019, Type.Write, Func.Holding_Register, "ControlBox Analog Output 2 Type    ", new Point(01, 1), "0 : Current, 1 : Voltage							", false),
            new Data(021, Type.Read , Func.Holding_Register, "Tool Digital Input(1~6)		     ", new Point(01, 1), "0b11 1111			   							", true),
            new Data(022, Type.Write, Func.Holding_Register, "Tool Digital Output(1~6) 		     ", new Point(01, 1), "0b11 1111			   							", true),                           

            new Data(256, Type.Read , Func.Holding_Register, "Controller_Major_Version		     ", new Point(10, 0), "						   							", true),
            new Data(257, Type.Read , Func.Holding_Register, "Controller Minor Version		     ", new Point(10, 0), "						   							", true),
            new Data(258, Type.Read , Func.Holding_Register, "Controller Patch Version		     ", new Point(10, 0), "						   							", true),
            new Data(259, Type.Read , Func.Holding_Register, "Robot State					     ", new Point(01, 1), "						   							", true),
            new Data(260, Type.Read , Func.Holding_Register, "Servo On Robot				     ", new Point(01, 1), "						   							", true),
            new Data(261, Type.Read , Func.Holding_Register, "Emergency Stopped				     ", new Point(01, 1), "						   							", true),
            new Data(262, Type.Read , Func.Holding_Register, "Safety Stopped				     ", new Point(01, 1), "						   							", true),
            new Data(263, Type.Read , Func.Holding_Register, "Direct Teach Button Pressed	     ", new Point(01, 1), "						   							", true),
            new Data(264, Type.Read , Func.Holding_Register, "Power Button Pressed			     ", new Point(01, 1), "						   							", true),
            new Data(270, Type.Read , Func.Holding_Register, "Joint Position 1				     ", new Point(10, 1), "[tenth of degree], signed Data					", true),
            new Data(271, Type.Read , Func.Holding_Register, "Joint Position 2				     ", new Point(10, 1), "[tenth of degree], signed Data					", true),
            new Data(272, Type.Read , Func.Holding_Register, "Joint Position 3				     ", new Point(10, 1), "[tenth of degree], signed Data					", true),
            new Data(273, Type.Read , Func.Holding_Register, "Joint Position 4				     ", new Point(10, 1), "[tenth of degree], signed Data					", true),
            new Data(274, Type.Read , Func.Holding_Register, "Joint Position 5				     ", new Point(10, 1), "[tenth of degree], signed Data					", true),
            new Data(275, Type.Read , Func.Holding_Register, "Joint Position 6				     ", new Point(10, 1), "[tenth of degree], signed Data					", true),
            new Data(280, Type.Read , Func.Holding_Register, "Joint Velocity 1				     ", new Point(10, 1), "[tenth of degree/s], signed Data					", true),
            new Data(281, Type.Read , Func.Holding_Register, "Joint Velocity 2				     ", new Point(10, 1), "[tenth of degree/s], signed Data					", true),
            new Data(282, Type.Read , Func.Holding_Register, "Joint Velocity 3				     ", new Point(10, 1), "[tenth of degree/s], signed Data					", true),
            new Data(283, Type.Read , Func.Holding_Register, "Joint Velocity 4				     ", new Point(10, 1), "[tenth of degree/s], signed Data					", true),
            new Data(284, Type.Read , Func.Holding_Register, "Joint Velocity 5				     ", new Point(10, 1), "[tenth of degree/s], signed Data					", true),
            new Data(285, Type.Read , Func.Holding_Register, "Joint Velocity 6				     ", new Point(10, 1), "[tenth of degree/s], signed Data					", true),
            new Data(290, Type.Read , Func.Holding_Register, "Joint Motor Current 1			     ", new Point(10, 3), "[mA]												", true),
            new Data(291, Type.Read , Func.Holding_Register, "Joint Motor Current 2			     ", new Point(10, 3), "[mA]												", true),
            new Data(292, Type.Read , Func.Holding_Register, "Joint Motor Current 3			     ", new Point(10, 3), "[mA]												", true),
            new Data(293, Type.Read , Func.Holding_Register, "Joint Motor Current 4			     ", new Point(10, 3), "[mA]												", true),
            new Data(294, Type.Read , Func.Holding_Register, "Joint Motor Current 5			     ", new Point(10, 3), "[mA]												", true),
            new Data(295, Type.Read , Func.Holding_Register, "Joint Motor Current 6			     ", new Point(10, 3), "[mA]												", true),
            new Data(300, Type.Read , Func.Holding_Register, "Joint Motor Temperature 1		     ", new Point(10, 0), "[℃]												", true),
            new Data(301, Type.Read , Func.Holding_Register, "Joint Motor Temperature 2		     ", new Point(10, 0), "[℃]												", true),
            new Data(302, Type.Read , Func.Holding_Register, "Joint Motor Temperature 3		     ", new Point(10, 0), "[℃]												", true),
            new Data(303, Type.Read , Func.Holding_Register, "Joint Motor Temperature 4		     ", new Point(10, 0), "[℃]												", true),
            new Data(304, Type.Read , Func.Holding_Register, "Joint Motor Temperature 5		     ", new Point(10, 0), "[℃]												", true),
            new Data(305, Type.Read , Func.Holding_Register, "Joint Motor Temperature 6		     ", new Point(10, 0), "[℃]												", true),
            new Data(310, Type.Read , Func.Holding_Register, "Joint Torque 1				     ", new Point(10, 0), "[N/m], signed data								", true),
            new Data(311, Type.Read , Func.Holding_Register, "Joint Torque 2				     ", new Point(10, 0), "[N/m], signed data								", true),
            new Data(312, Type.Read , Func.Holding_Register, "Joint Torque 3				     ", new Point(10, 0), "[N/m], signed data								", true),
            new Data(313, Type.Read , Func.Holding_Register, "Joint Torque 4				     ", new Point(10, 0), "[N/m], signed data								", true),
            new Data(314, Type.Read , Func.Holding_Register, "Joint Torque 5				     ", new Point(10, 0), "[N/m], signed data								", true),
            new Data(315, Type.Read , Func.Holding_Register, "Joint Torque 6				     ", new Point(10, 0), "[N/m], signed data								", true),
            new Data(400, Type.Read , Func.Holding_Register, "Task Position X				     ", new Point(10, 1), "[tenth of mm], in base frame, signed data		", true),
            new Data(401, Type.Read , Func.Holding_Register, "Task Position Y				     ", new Point(10, 1), "[tenth of mm], in base frame, signed data		", true),
            new Data(402, Type.Read , Func.Holding_Register, "Task Position Z				     ", new Point(10, 1), "[tenth of mm], in base frame, signed data		", true),
            new Data(403, Type.Read , Func.Holding_Register, "Task Orientation A			     ", new Point(10, 1), "[tenth of degree], in base frame, signed data	", true),
            new Data(404, Type.Read , Func.Holding_Register, "Task Orientation B			     ", new Point(10, 1), "[tenth of degree], in base frame, signed data	", true),
            new Data(405, Type.Read , Func.Holding_Register, "Task Orientation C			     ", new Point(10, 1), "[tenth of degree], in base frame, signed data	", true),
            new Data(410, Type.Read , Func.Holding_Register, "Task Velocity X				     ", new Point(10, 1), "[tenth of mm/s], in base frame, signed data		", true),
            new Data(411, Type.Read , Func.Holding_Register, "Task Velocity Y				     ", new Point(10, 1), "[tenth of mm/s], in base frame, signed data		", true),
            new Data(412, Type.Read , Func.Holding_Register, "Task Velocity Z				     ", new Point(10, 1), "[tenth of mm/s], in base frame, signed data		", true),
            new Data(413, Type.Read , Func.Holding_Register, "Task Angular Velocity RX		     ", new Point(10, 1), "[tenth of degree/s], in base frame, signed data	", true),
            new Data(414, Type.Read , Func.Holding_Register, "Task Angular Velocity RY		     ", new Point(10, 1), "[tenth of degree/s], in base frame, signed data	", true),
            new Data(415, Type.Read , Func.Holding_Register, "Task Angular Velocity RZ		     ", new Point(10, 1), "[tenth of degree/s], in base frame, signed data	", true),
            new Data(420, Type.Read , Func.Holding_Register, "Tool Offset Length X			     ", new Point(10, 1), "[tenth of mm], in tool frame, signed data		", true),
            new Data(421, Type.Read , Func.Holding_Register, "Tool Offset Length Y			     ", new Point(10, 1), "[tenth of mm], in tool frame, signed data		", true),
            new Data(422, Type.Read , Func.Holding_Register, "Tool Offset Length Z			     ", new Point(10, 1), "[tenth of mm], in tool frame, signed data		", true),
            new Data(423, Type.Read , Func.Holding_Register, "Tool Offset Degree A			     ", new Point(10, 1), "[tenth of degree], in tool frame, signed data	", true),
            new Data(424, Type.Read , Func.Holding_Register, "Tool Offset Degree B			     ", new Point(10, 1), "[tenth of degree], in tool frame, signed data	", true),
            new Data(425, Type.Read , Func.Holding_Register, "Tool Offset Degree C			     ", new Point(10, 1), "[tenth of degree], in tool frame, signed data	", true),
            new Data(430, Type.Read , Func.Holding_Register, "Task External Force X			     ", new Point(10, 1), "[N], in base frame, signed data					", true),
            new Data(431, Type.Read , Func.Holding_Register, "Task External Force Y			     ", new Point(10, 0), "[N], in base frame, signed data					", true),
            new Data(432, Type.Read , Func.Holding_Register, "Task External Force Z			     ", new Point(10, 0), "[N], in base frame, signed data					", true),
            new Data(433, Type.Read , Func.Holding_Register, "Task External Moment X		     ", new Point(10, 0), "[Nm], in base frame, signed data					", true),
            new Data(434, Type.Read , Func.Holding_Register, "Task External Moment Y		     ", new Point(10, 0), "[Nm], in base frame, signed data					", true),
            new Data(435, Type.Read , Func.Holding_Register, "Task External Moment Z		     ", new Point(10, 0), "[Nm], in base frame, signed data					", true),

            //START
            //Write Sector
            new Data(128, Type.Write,  Func.Holding_Register, "Manual, Auto, Recipe, Sync Command", new Point(01, 1), "-Command Bit 	   							    ", true),
            new Data(129, Type.Write,  Func.Holding_Register, "Manual Pos Move Command    		 ", new Point(01, 1), "-Command Bit 	   							    ", true),
            new Data(130, Type.Write,  Func.Holding_Register, "Tool Weight              		 ", new Point(01, 1), "-Command Data 	   							    ", true),
            new Data(131, Type.Write,  Func.Holding_Register, "Pick & Place               		 ", new Point(01, 1), "-Status Data                                     ", true),
            new Data(132, Type.Write,  Func.Holding_Register, "X-Axis Position         		     ", new Point(01, 1), "-Status                                          ", true),
            new Data(133, Type.Write,  Func.Holding_Register, "Auto Position         		     ", new Point(01, 1), "-Command Data                                    ", true),
            new Data(134, Type.Write,  Func.Holding_Register, "Zimmer Gripper Oper         	     ", new Point(01, 1), "-Command Data                                    ", true),


            new Data(139, Type.Write,  Func.Holding_Register, "Shaking Count               		 ", new Point(01, 1), "-Command Data 	   							    ", true),
            new Data(140, Type.Write,  Func.Holding_Register, "Oxzen Time               		 ", new Point(01, 1), "-Command Data 	   							    ", true),
            new Data(141, Type.Write,  Func.Holding_Register, "Drain Count               		 ", new Point(01, 1), "-Command Data 	   							    ", true),
            new Data(142, Type.Write,  Func.Holding_Register, "Basket shake Count          		 ", new Point(01, 1), "-Command Data 	   							    ", true),
            new Data(143, Type.Write,  Func.Holding_Register, "Cobot Speed               		 ", new Point(01, 1), "-Command Data 	   							    ", true),
                                                                                                                                                                        
                                                                                                                                                                        
                                                                                                                                                                        
            //Read Sector                                                                                                                                               
            new Data(144, Type.Read,  Func.Holding_Register, "Manual, Auto, Recipe, Sync Complted", new Point(01, 1), "-Complted Bit    							    ", true),
            new Data(145, Type.Read,  Func.Holding_Register, "Manual Pos Move Complted    		 ", new Point(01, 1), "-Complted Bit    							    ", true),
            new Data(146, Type.Read,  Func.Holding_Register, "Tool Weight Status          		 ", new Point(01, 1), "-Status           							    ", true),
            new Data(147, Type.Read,  Func.Holding_Register, "Chiken Input Start Status   		 ", new Point(01, 1), "-Status           							    ", true),
                                                                                                                                                                        
            new Data(149, Type.Read,  Func.Holding_Register, "Chiken Auto Process Complted		 ", new Point(01, 1), "-Complted Bit       							    ", true),
            new Data(150, Type.Read,  Func.Holding_Register, "Zimmer Gripper Oper         	     ", new Point(01, 1), "-Complted Bit                                    ", true),
            new Data(153, Type.Read,  Func.Holding_Register, "Manual Step Number           		 ", new Point(01, 1), "-Debug            							    ", true),
            new Data(154, Type.Read,  Func.Holding_Register, "Auto Step Number          		 ", new Point(01, 1), "-Debug              							    ", true),

            //new Data(155, Type.Read,  Func.Holding_Register, "Robot Manual Action Flag     		 ", new Point(01, 1), "-Debug           							", true),
            //new Data(156, Type.Read,  Func.Holding_Register, "Robot Auto Action Flag     		 ", new Point(01, 1), "-Debug           							", true),
            //END


            //new Data(128, Type.Read , Func.Holding_Register, "General purpose 16bit registers", new Point(10, 0), "						   							", true),
            //new Data(128, Type.Read , Func.Holding_Register, "General purpose 16bit registers", new Point(10, 0), "						   							", true),
            //new Data(128, Type.Read , Func.Holding_Register, "General purpose 16bit registers", new Point(10, 0), "						   							", true),
            //new Data(128, Type.Read , Func.Holding_Register, "General purpose 16bit registers", new Point(10, 0), "						   							", true),
            //new Data(128, Type.Read , Func.Holding_Register, "General purpose 16bit registers", new Point(10, 0), "						   							", true),
            //new Data(128, Type.Read , Func.Holding_Register, "General purpose 16bit registers", new Point(10, 0), "						   							", true),
            //new Data(128, Type.Read , Func.Holding_Register, "General purpose 16bit registers", new Point(10, 0), "						   							", true),
            //new Data(128, Type.Read , Func.Holding_Register, "General purpose 16bit registers", new Point(10, 0), "						   							", true),
            //new Data(128, Type.Read , Func.Holding_Register, "General purpose 16bit registers", new Point(10, 0), "						   							", true),
            //new Data(128, Type.Read , Func.Holding_Register, "General purpose 16bit registers", new Point(10, 0), "						   							", true),
            //new Data(128, Type.Read , Func.Holding_Register, "General purpose 16bit registers", new Point(10, 0), "						   							", true),
            //new Data(128, Type.Read , Func.Holding_Register, "General purpose 16bit registers", new Point(10, 0), "						   							", true),
            //new Data(128, Type.Read , Func.Holding_Register, "General purpose 16bit registers", new Point(10, 0), "						   							", true),
            //new Data(128, Type.Read , Func.Holding_Register, "General purpose 16bit registers", new Point(10, 0), "						   							", true),
             };

        public static byte SetBit(int dat)
        {
            if (dat == 1)
            {
                return 0xff;
            }
            return 0;
        }

        public partial class Data
        {
            /// <summary>
            /// 최초 생성자
            /// </summary>
            /// <param name="addr"></param>
            /// <param name="rw"></param>
            /// <param name="resistor"></param>
            /// <param name="desc"></param>
            /// <param name="pt"></param>
            /// <param name="comment"></param>
            public Data(int addr, Type rw, Func resistor, string desc, Point pt, string comment, bool use)
            {
                Address = addr;
                type = rw;
                func = resistor;
                strDesc = desc;
                ptDecimal = pt;
                strComment = comment;
                IsUsed = use;
            }

            public Data(int addr, int data)
            {
                Address = addr;
                iData = data;
            }

            public bool IsUsed;
            public int Address;            
            public Type type;
            public Func func;
            public int iData;
            public string strData;
            public string strDesc;
            public Point ptDecimal;
            public string strComment;

            //이전 데이터 업데이트 변경시에만 하기위하여 사용한다.
            public int iPrevData;
            public bool IsFirst = false;

            public int iCurrData;
        }

        public enum Type
        { 
            Read, Write, Both
        }

        public enum Func
        {
            Read_Coil= 0x01, Holding_Register=0x03, Write_Coil=0x05, Write_Register = 0x06,
        }

        public enum Robot_Write
        {
            GPIO, TOOL_IO
        }

        public enum RobotState_Ver_1_0
        {
            INITIALIZING = 0,
            STANDBY = 1,
            OPERATING = 2,
            SAFE_OFF = 3,
            TECHING = 4,
            SAFE_STOP = 5,
            EMERGENCY_STOP = 6,
            HOMING = 7,
            RECOVERY = 8,
            SAFE_STOP_2 = 9,
            SAFE_OFF2 = 10,                        
            NOT_READY = 15,            
        }

        public enum RobotState_Ver_1_1
        {
            BACKDRIVE_HOLD = 0,
            BACKDRIVE_RELEASE = 1,
            BACKDRIVE_RELEASE_BY_COCKPIT = 2,
            SAFE_OFF = 3,
            INITIALIZING = 4,
            INTERRUPTED = 5,
            EMERGENCY_STOP = 6,
            AUTO_MEASURE = 7,
            RECOVERY_STANDBY = 8,
            RECOVERY_JOGGING = 9,
            RECOVERY_HANDGUIDING = 10,
            MANUAL_STANDBY = 11,
            MANUAL_JOGGING = 12,
            MANUAL_HANDGUIDING = 13,
            HIGH_PRIORITY_RUNNING = 14,
            STANDALONE_STANDBY = 15,
            STANDALONE_RUNNING = 16,
            COLLABORATIVE_STANDBY = 17,
            COLLABORATIVE_RUNNING = 18,
            HANDGUIDING_CONTROL_STANDBY = 19,
            HANDGUIDING_CONTROL_RUNNING = 20,
        }

    }
}
