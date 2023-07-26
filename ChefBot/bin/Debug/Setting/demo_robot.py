# [ Information ]---------------------------------------------------------------------------------------------------
# Project Code : 2304002-GM-HM
# Cobot Pregram Version : A0509 GV02100001
# Taske Writer : ChefBot_20230501
# Gripper : Zimmer GEP2016

# [ 업데이트 및 변경 사항 ]---------------------------------------------------------------------------------------------------------
# 날짜	/	작성자	/	변경사유
# 20230426	/	정상진	/	최초 작성
# 2023.05.25 정명재 자동 인터페이스 변경

# [ 변수 설명 ]------------------------------------------------------------------------------------------------------------------
L_Auto_Mode=1				# 자동 운전 모드
L_Manual_Mode=0				# 수동 운전 모드

# 프로그램 작성 시작 -------------------------------------------------------------------------------------------------------------
# [ Communication Program Start ] :::::::::::::::::::::::::::::::::::::::::::::::::::::::::
# 1-1. Modbus DATA Read [ Modbus Address 128 ~ 143 ]
# [ PC로 부터 받는 모드버스 128번지 (Request) 이벤트 확인 ]
Global_M_Read_Data[128]=get_modbus_slave(128)
# Read128 Bit Index 0 : Manual Mode CMD ( 수동 운전 모드 전환 요청 비트 )
L_M_Read_128_Data_Bit_Index_0 = Global_M_Read_Data[128]>>0 &1
# Read128 Bit Index 1 : Auto Mode CMD ( 자동 운전 모드 전환 요청 비트 )
L_M_Read_128_Data_Bit_Index_1 = Global_M_Read_Data[128]>>1 &1
# Read128 Bit Index 2 : Basket Shake Mode CMD ( 투입 후 Basket Shake 모드 전환 요청 비트 ( 0 미사용, 1 사용 ) )
L_M_Read_128_Data_Bit_Index_2 = Global_M_Read_Data[128]>>2 &1
# Read128 Bit Index 3 : Gripper USE CMD ( Gripper 사용 유/무 ( 0 : 미사용, 1 : 사용 )
L_M_Read_128_Data_Bit_Index_3 = Global_M_Read_Data[128]>>3 &1
# Read128 Bit Index 4 : Oil Drain Mode USE CMD ( 배출 후 바스킷 기름 빼기 모드 이벤트 확인 ( 0 : 미사용, 1 : 사용 ))
L_M_Read_128_Data_Bit_Index_4 = Global_M_Read_Data[128]>>4 &1
# Read128 Bit Index 5 : 없음
L_M_Read_128_Data_Bit_Index_5 = Global_M_Read_Data[128]>>5 &1
# Read128 Bit Index 6 : 없음
L_M_Read_128_Data_Bit_Index_6 = Global_M_Read_Data[128]>>6 &1
# Read128 Bit Index 7 : 없음
L_M_Read_128_Data_Bit_Index_7 = Global_M_Read_Data[128]>>7 &1
# Read128 Bit Index 8 : 없음
L_M_Read_128_Data_Bit_Index_8 = Global_M_Read_Data[128]>>8 &1
# Read128 Bit Index 9 : 없음
L_M_Read_128_Data_Bit_Index_9 = Global_M_Read_Data[128]>>9 &1
# Read128 Bit Index 10 : 없음
L_M_Read_128_Data_Bit_Index_10 = Global_M_Read_Data[128]>>10 &1
# Read128 Bit Index 11 : 없음
L_M_Read_128_Data_Bit_Index_11 = Global_M_Read_Data[128]>>11 &1
# Read128 Bit Index 12 : 없음
L_M_Read_128_Data_Bit_Index_12 = Global_M_Read_Data[128]>>12 &1
# Read128 Bit Index 13 : Initial CMD ( 초기화 명령 )
L_M_Read_128_Data_Bit_Index_13 = Global_M_Read_Data[128]>>13 &1
# Read128 Bit Index 14 : Recipe Download CMD ( 레시피 다운로드 요청 비트  )
L_M_Read_128_Data_Bit_Index_14 = Global_M_Read_Data[128]>>14 &1
# Read128 Bit Index 15 : PC Sync CMD ( PC 동기화 요청 비트  )
L_M_Read_128_Data_Bit_Index_15 = Global_M_Read_Data[128]>>15 &1
# [ 투입 후 바스킷 흔들기 모드 이벤트 확인 ]
# Cobot Basket Shake 사용(1) 명령인 경우 
if	L_M_Read_128_Data_Bit_Index_2 == 1:
	# 자동 운전 모드에서 사용 되는 Gripper 사용 변수를 사용(1)으로 업데이트 한다.
	Global_C_Basket_Shake = 1
# Cobot Basket Shake 미사용(0) 명령인 경우 
elif	L_M_Read_128_Data_Bit_Index_2 == 0:
	# 자동 운전 모드에서 사용 되는 Gripper 사용 변수를 미사용(0)으로 업데이트 한다.
	Global_C_Basket_Shake = 0

# [ Cobot Grip Use 이벤트 확인 ]
# Cobot Gripper 사용 명령인 경우 
if	L_M_Read_128_Data_Bit_Index_3 == 1:
	# 자동 운전 모드에서 사용 되는 Gripper 사용 변수를 사용(1)으로 업데이트 한다.
	Global_C_Gripper_Use = 1
# Cobot Gripper 미사용(0) 사용 명령인 경우
elif	L_M_Read_128_Data_Bit_Index_3 == 0:
	# 자동 운전 모드에서 사용 되는 Gripper 사용 변수를 미사용(0)으로 업데이트 한다.
	Global_C_Gripper_Use = 0

# [ 배출 후 바스킷 기름 빼기 모드 이벤트 확인 ]
# Cobot Basket Shake 사용(1) 명령인 경우 
if	L_M_Read_128_Data_Bit_Index_4 == 1:
	# 자동 운전 모드에서 사용 되는 Gripper 사용 변수를 사용(1)으로 업데이트 한다.
	Global_C_Oil_Drain = 1
# Cobot Basket Shake 미사용(0) 명령인 경우 
elif	L_M_Read_128_Data_Bit_Index_4 == 0:
	# 자동 운전 모드에서 사용 되는 Gripper 사용 변수를 미사용(0)으로 업데이트 한다.
	Global_C_Oil_Drain = 0

# [ Cobot 초기화 이벤트 확인 ]
# Cobot 초기화 명령인 경우 
if	L_M_Read_128_Data_Bit_Index_13 == 1:
	# 초기화 변수를 사용(1)으로 업데이트 한다.
	Global_C_Initial = 1
# Cobot 초기화 명령이 아닌 경우 
elif	L_M_Read_128_Data_Bit_Index_13 == 0:
	# 초기화 변수를 미사용(0)으로 업데이트 한다.
	Global_C_Initial = 0

# [ PC로 부터 받는 모드버스 130번지 (Cobot Tool Weight) 이벤트 확인 ]
Global_M_Read_Data[130]=get_modbus_slave(130)
# Cobot Tool Weight 값이 현재 0이 아닌 경우
if	Global_M_Read_Data[130] != 0:
	# 모드 버스 130번지에서 읽은 Cobot Tool Weight 값을 적용 한다. 
	Global_C_Tool_Weight = Global_M_Read_Data[130] 	 

# [ PC로 부터 받는 모드버스 139번지 (조리 중 바스켓 흔들기 카운트 값) 이벤트 확인 ]
Global_M_Read_Data[139]=get_modbus_slave(139)
# Cobot 조리 중 바스켓 흔들기 카운트 값이 0이 아닌 경우
if	Global_M_Read_Data[139] != 0:
	# 모드 버스 139번지에서 읽은 Cobot 운전 속도 값을 적용 한다.
	Global_C_Basket_Shake_Count_B = Global_M_Read_Data[139]

# [ PC로 부터 받는 모드버스 140번지 (조리 중 바스켓 들어 올리기 시간) 이벤트 확인 ]
Global_M_Read_Data[140]=get_modbus_slave(140)
# Cobot 조리 중 바스켓 들어 올리기 ( 산소 입히기 ) 시간 값이 0이 아닌 경우
if	Global_M_Read_Data[140] != 0:
	# 모드 버스 140번지에서 읽은 Cobot 조리 중 바스켓 들어 올리기( 산소 입히기 ) 시간 값을 적용 한다.
	Global_C_Timer = Global_M_Read_Data[140]

# [ PC로 부터 받는 모드버스 141번지 (조리 완료 후 바스켓 배출 후 기름 빼기 카운트) 이벤트 확인 ]
Global_M_Read_Data[141]=get_modbus_slave(141)
# Cobot 조리 완료 후 바스켓 배출 후 기름 빼기 카운트 값이, 0이 아닌 경우
if	Global_M_Read_Data[141] != 0:
	# 모드 버스 141번지에서 읽은 Cobot 조리 완료 후 바스켓 배출 후 기름 빼기 카운트 값을 적용 한다.
	Global_C_Oil_Drain_Count = Global_M_Read_Data[141]

# [ PC로 부터 받는 모드버스 142번지 (Cobot 투입 후 바스켓 흔들기 카운트) 이벤트 확인 ]
Global_M_Read_Data[142]=get_modbus_slave(142)
# Cobot 투입 후 바스켓 흔들기 카운트 값이 현재 값이 0이 아닌 경우
if	Global_M_Read_Data[142] != 0:
	# 모드 버스 142번지에서 읽은 Cobot 투입 후 바스켓 흔들기 카운트 값을 적용 한다.
	Global_C_Basket_Shake_Count_A = Global_M_Read_Data[142]

# [ PC로 부터 받는 모드버스 143번지 (Cobot 운전 속도) 이벤트 확인 ]
Global_M_Read_Data[143]=get_modbus_slave(143)
# Cobot Recipe 운전 속도 값이 현재 값이 0이 아닌 경우
if	Global_M_Read_Data[143] != 0:
	# 모드 버스 143번지에서 읽은 Cobot 운전 속도 값을 적용 한다.
	Global_C_Operation_Speed = Global_M_Read_Data[143]
	# Cobot 속도 변경 함수 실행
	change_operation_speed(Global_C_Operation_Speed)
	# Cobot 속도 상태 변수 업데이트
	Global_S_Operation_Speed = Global_C_Operation_Speed

# 1-2. Modbus Write [ Modbus Address 144 ~ 159 ]
# [ PC로 쓰는 모드버스 144번지 (Cobot 투입 후 바스킷 흔들기 상태) 이벤트 확인 ]
# 투입 후 바스킷 흔들기 사용 모드가 사용(1)이고 투입 후 바스킷 흔들기 사용 모드가 사용(1) 상태이면,
if	Global_C_Basket_Shake == 1 and Global_S_Basket_Shake == 1 :
	# Write144 Bit Index 3 : Basket Shake Mode Status ( 투입 후 Basket Shake 모드 전환 상태 비트를 사용으로 업데이트 한다. )
	Global_M_Write_Data[144] |= 0x4 
	#모드 버스 144번지에 값을 적용 한다.
	set_modbus_slave(144, Global_M_Write_Data[144]) 
# 투입 후 바스깃 흔들기 사용 모드가 미사용(0)이고 투입후 바스킷 흔들기 사용 모드 상태가 미사용(0) 상태이면,
elif	Global_C_Basket_Shake == 0 and Global_S_Basket_Shake == 0 :
	# 16비트 (FFFF)에서 해당 비트 (0x8) 를 빼준 값을 넣는다.
	Global_M_Write_Data[144] &= 0xfffb
	#모드 버스 144번지에 값을 적용 한다.
	set_modbus_slave(144, Global_M_Write_Data[144]) 

# [ PC로 쓰는 모드버스 144번지 (Cobot Gripper Use 상태) 이벤트 확인 ]
# Gripper 사용 모드가 사용(1)이고 Gripper 사용 모드가 사용(1) 상태이면,
if	Global_C_Gripper_Use == 1 and Global_S_Gripper_Use == 1 :
	# Write144 Bit Index 4 : Gripper USE Status ( Gripper 사용 모드 상태 비트를 사용으로 업데이트 한다. )
	Global_M_Write_Data[144] |= 0x8 
	#모드 버스 144번지에 값을 적용 한다.
	set_modbus_slave(144, Global_M_Write_Data[144]) 
# Gripper 사용 모드가 미사용(0)이고 Gripper 사용 모드 상태가 미사용(0) 상태이면,
elif	Global_C_Gripper_Use == 0 and Global_S_Gripper_Use == 0 :
	# 16비트 (FFFF)에서 해당 비트 (0x8) 를 빼준 값을 넣는다.
	Global_M_Write_Data[144] &= 0xfff7
	#모드 버스 144번지에 값을 적용 한다.
	set_modbus_slave(144, Global_M_Write_Data[144]) 

# [ PC로 쓰는 모드버스 144번지 (Cobot 배출 후 기름 빼기 상태) 이벤트 확인 ]
# 기름 빼기 모드가 사용(1)이고 기름 빼기 모드 상태가 사용(1) 상태이면,
if	Global_C_Oil_Drain == 1 and Global_S_Oil_Drain == 1 :
	# Write144 Bit Index 3 : Oil Drain Mode USE Status ( 배출 후 바스킷 기름 빼기 모드 상태 비트 를 사용 으로 업데이트 한다. )
	Global_M_Write_Data[144] |= 0x10 
	#모드 버스 144번지에 값을 적용 한다.
	set_modbus_slave(144, Global_M_Write_Data[144]) 
# 기름 빼기 모드가 미사용(0)이고 기름 빼기 모드 상태가 미사용(0) 상태이면,
elif	Global_C_Oil_Drain == 0 and Global_S_Oil_Drain == 0 :
	# 16비트 (FFFF)에서 해당 비트 (0x10) 를 빼준 값을 넣는다.
	Global_M_Write_Data[144] &= 0xffef
	#모드 버스 144번지에 값을 적용 한다.
	set_modbus_slave(144, Global_M_Write_Data[144]) 

# [ PC로 쓰는 모드버스 144번지 (Cobot 초기화 실행 완료 상태) 이벤트 확인 ]
# 초기화 명령(1)이고, 초기화 상태 완료 변수가(1)이고, 초기화 상태가 확인 변수가 0이면,
if	Global_C_Initial == 1 and Global_S_Initial_Complate == 1 and Global_S_Initial == 0 :
	# 초기화 상태가 확인 변수를 1로 업데이트 한다.
	Global_S_Initial = 1
# 초기화 명령 (1)이고, 초기화 상태 확인 변수가 0이 아니면,
elif Global_S_Initial == 1 and (Global_M_Write_Data[144] & 0x2000)!=0:
	# Write144 Bit Index 14 : Initial Status ( 초기화 상태 비트 )
	Global_M_Write_Data[144] |= 0x2000
	#모드 버스 144번지에 값을 적용 한다.
	set_modbus_slave(144, Global_M_Write_Data[144]) 
elif Global_S_Initial==0 and Global_S_Initial_Complate == 0 :
	# 16비트 (FFFF)에서 해당 비트 (0x2000) 를 빼준 값을 넣는다.
	Global_M_Write_Data[144] &= 0xdfff
	#모드 버스 144번지에 값을 적용 한다.
	set_modbus_slave(144, Global_M_Write_Data[144]) 

# [ PC로 쓰는 모드버스 145번지 (수동 운전 완료 위치 상태) 이벤트 확인  ]
# Cobot 수동 운전 완료 위치 상태 값이 업데이트 될 때만 
#if Global_M_Write_Data[145]!= Global_S_Manual_Position_Bit:
	#Global_M_Write_Data[145] = Global_S_Manual_Position_Bit
	# 모드 버스 145번지에 값을 적용 한다.
	#set_modbus_slave(145, Global_M_Write_Data[145]) 

# [ PC로 쓰는 모드버스 146번지 (Cobot Tool Weight 설정 상태) 이벤트 확인  ]
# Cobot Tool Weight 상태 값이 업데이트 될 때만 
if	Global_M_Write_Data[146] != Global_S_Tool_Weight:
	# 변수를 업데이트 한다.
	Global_M_Write_Data[146] = Global_S_Tool_Weight
	# 모드 버스 146번지에 값을 적용 한다.
	set_modbus_slave(146, Global_M_Write_Data[146]) 
   
# [ PC로 쓰는 모드버스 155번지 (Cobot 조리 중 후 바스킷 흔들기 카운트 설정 상태) 이벤트 확인  ]
# Cobot 조리 중 후 바스킷 흔들기 카운트 설정 상태 값이 현재 값과 같지 않은 경우
if	Global_M_Write_Data[155] != Global_S_Basket_Shake_Count_B:
	# Modbus 155 번지에 현재 설정 된 배출 후 바스킷 흔들기 카운트 값을 입력 한다.
	Global_M_Write_Data[155] = Global_S_Basket_Shake_Count_B
	#모드 버스 146번지에 값을 적용 한다.
	set_modbus_slave(155, Global_M_Write_Data[155])

# [ PC로 쓰는 모드버스 156번지 (Cobot 조리 중 후 바스킷 들어 올리기 시간 설정 상태) 이벤트 확인  ]
# [ Cobot 조리 중 바스켓 들어 올리기 ( 산소 입히기 ) 시간 변경 완료 상태 쓰기 ]
# 조리 중 바스켓 들어 올리기 ( 산소 입히기 ) 시간 값이 현재 값과 같지 않고, 0이 아닌 경우
if	Global_M_Write_Data[156] != Global_S_Timer:
	# Modbus 156 번지에 현재 설정 된 배출 후 바스킷 흔들기 카운트 값을 입력 한다.
	Global_M_Write_Data[156] = Global_S_Timer
	# 모드 버스 156번지에 값을 적용 한다.
	set_modbus_slave(156, Global_M_Write_Data[156]) 

# [ PC로 쓰는 모드버스 157번지 (Cobot 조리 완료 후 바스킷 기름 빼기 카운트 설정 상태) 이벤트 확인  ]    
# Cobot 조리 완료 후 바스킷 기름 빼기 카운트 값이 현재 값과 같지 않은 경우
if	Global_M_Write_Data[157] != Global_S_Oil_Drain_Count:
	# Modbus 157 번지에 현재 설정 된 배출 후 바스킷 흔들기 카운트 값을 입력 한다.
	Global_M_Write_Data[157] = Global_S_Oil_Drain_Count
	# 모드 버스 157번지에 값을 적용 한다.
	set_modbus_slave(157, Global_M_Write_Data[157]) 
    
# [ PC로 쓰는 모드버스 158번지 (Cobot 투입 후 바스킷 흔들기 카운트 설정 상태) 이벤트 확인  ]    
# Cobot 투입 후 바스킷 흔들기 카운트 설정 값이 현재 값과 같지 않은 경우
if	Global_M_Write_Data[158] != Global_S_Basket_Shake_Count_A:
	# Modbus 158 번지에 현재 설정 된 배출 후 바스킷 흔들기 카운트 값을 입력 한다.	
	Global_M_Write_Data[158] = Global_S_Basket_Shake_Count_A
	# 모드 버스 158번지에 값을 적용 한다.
	set_modbus_slave(158, Global_M_Write_Data[158])   

# Cobot 속도 상태 변수 업데이트
if	Global_M_Write_Data[159] != Global_S_Operation_Speed:
	# Modbus 159 번지에 현재 설정 된 Cobot 속도 설정 값을 입력 한다.	
	Global_M_Write_Data[159]=Global_S_Operation_Speed
	# 모드 버스 159번지에 값을 적용 한다.
	set_modbus_slave(159, Global_M_Write_Data[159])     

# [ Communication Program END ] :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


# [ 자동/수동 운전 모드 이벤트 확인 Program Start ] :::::::::::::::::::::::::::::::::::::::::::::::
# 2-1. 4번 보드의 Digital Input 16번 Bit를 Global_CMD_IO_Bit 변수에 넣는다.
#2023.05.25 EDIT : MJ 
#Global_C_IO_Bit=get_digital_input(16)
Global_C_IO_Bit=get_digital_input(10)

# 2-2. 수동 운전 프로세스 스텝 초기화
# Cobot 수동 동작 위치 비트 읽기
Global_M_Read_Data[129]=get_modbus_slave(129)

#Cobot 수동 명령 실행 비트가 0이면,
if	Global_M_Read_Data[129] == 0:
	# 수동 운전 프로세스 스텝을 0으로 업데이트 한다.
	Global_S_Manual_Action_Step = 0
	# 변수에 현재 수동 운전 프로세스 스텝 값을 입력 한다.
	Global_M_Write_Data[153] = Global_S_Manual_Action_Step
	#모드 버스 153번지에 값을 적용 한다.
	set_modbus_slave(153, Global_M_Write_Data[153])

# 2-3. 비정상 동작 후 에러 처리를 하기 위해 현재 위치를 찾아야 한다.
if	Global_C_IO_Bit==1 and Global_S_IO_Bit==0:
	#Task 좌표 비교 옵셋 값
	L_P_Task_Const_Offset = 1.5
	# Global_P_Wait(대기) 위치 찾기
	Global_Position_Check=1
	L_Wait_X_min = (Global_P_Wait[0]-L_P_Task_Const_Offset)
	L_Wait_X_Max = (Global_P_Wait[0]+L_P_Task_Const_Offset)
	Global_Position_Check &= check_position_condition(DR_AXIS_X, L_Wait_X_min, L_Wait_X_Max, DR_BASE)
	L_Wait_Y_min = (Global_P_Wait[1]-L_P_Task_Const_Offset)
	L_Wait_Y_Max = (Global_P_Wait[1]+L_P_Task_Const_Offset)
	Global_Position_Check &= check_position_condition(DR_AXIS_Y, L_Wait_Y_min, L_Wait_Y_Max, DR_BASE)
	L_Wait_Z_min = (Global_P_Wait[2]-L_P_Task_Const_Offset)
	L_Wait_Z_Max = (Global_P_Wait[2]+L_P_Task_Const_Offset)
	Global_Position_Check &= check_position_condition(DR_AXIS_Z, L_Wait_Z_min, L_Wait_Z_Max, DR_BASE)
	if	Global_Position_Check == 1:
		# 현재 위치 값이 대기 위치 값과 같으면, 이전 명령을 1로 만든다.
		Global_S_Manual_Position_Bit = 0
# 상태 변수 업데이트
Global_S_IO_Bit=Global_C_IO_Bit
# [ 자동/수동 운전 모드 이벤트 확인 Program END ] :::::::::::::::::::::::::::::::::::::::::::::::::


# [ 자동 운전 Program Start ] ::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
# Local Variables Define
# Gripper_Weight : MQC + Gripper + JAW + 나사 = 1.9Kg
L_Tool_Weight_19 = 1900
# Basket_Weight : Basket + MQC + Gripper + JAW + 나사 = 2.8Kg
L_Tool_Weight_28 = 2800
# Bone05_Weight : 뼈 치킨 반마리 + Basket + MQC + Gripper + JAW + 나사 = 3.3Kg
L_Tool_Weight_33 = 3300
# Bone10_Weight : 뼈 치킨 한마리 + Basket + MQC + Gripper + JAW + 나사 = 3.8Kg
L_Tool_Weight_38 = 3800
# Bone15_Weight : 뼈 치킨 한마리반 + Basket + MQC + Gripper + JAW + 나사 = 4.3Kg
L_Tool_Weight_43 = 4300
# Boneless05_Weight : 순살 치킨 반마리 + Basket + MQC + Gripper + JAW + 나사 = 3.4Kg
L_Tool_Weight_34 = 3400		
# Boneless10_Weight : 순살 치킨 한마리 + Basket + MQC + Gripper + JAW + 나사 = 4.0Kg
L_Tool_Weight_40 = 4000
# Boneless15_Weight : 순살 치킨 한마리반 + Basket + MQC + Gripper + JAW + 나사 = 4.6Kg
L_Tool_Weight_46 = 4600
L_Wait = 1							# 대기 위치 이동 명령
L_Loader_A = 2						# Loader A 위치 이동 명령 Bit
L_Loader_B = 3						# Loader B 위치 이동 명령 Bit
L_Loader_C = 4						# Loader C 위치 이동 명령 Bit
L_Cooker_1_1 = 5					# Cooker 1-1 위치 이동 명령 Bit
L_Cooker_1_2 = 6					# Cooker 1-2 위치 이동 명령 Bit
L_Cooker_2_1 = 7					# Cooker 2-1 위치 이동 명령 Bit
L_Cooker_2_2 = 8					# Cooker 2-2 위치 이동 명령 Bit
L_Cooker_3_1 = 9					# Cooker 3-1 위치 이동 명령 Bit
L_Cooker_3_2 = 10					# Cooker 3-2 위치 이동 명령 Bit
L_Cooker_Function_A_Common = 17		# Cooker 1-1, 1-2, 2-1, 2-2, 3-1 위치 이동 명령 Bit
L_Cooker_Function_A_3_2 = 20		# Cooker 3-2 위치 이동 명령 Bit
L_Cooker_Function_B_Common = 19		# Cooker 1-1, 1-2, 2-1, 2-2, 3-1 위치 이동 명령 Bit
L_Cooker_Function_B_3_2 = 21		# Cooker 3-2 위치 이동 명령 Bit
L_CMD_BIT_1 = 12					# Digital Input 12
L_CMD_BIT_4 = 15					# Digital Input 15
L_Tool_Measure = 11					# Tool Weight Measure 위치 이동 명령
L_Tool_Change = 12					# Tool Change 위치 이동 명령
L_Home = 13							# Home 위치 이동 명령
L_Idle = 14							# Cobot Idle 위치 이동 명령
L_Align_Master_Left = 15			# Align Mastering Left 위치 이동 명령
L_Align_Master_Right = 16			# Align Mastering Right  위치 이동 명령	


L_Target_Pos = [0,0,0,0,0,0]
L_Interlock_Pos = [0,0,0,0,0,0]
# 운전 모드가 자동 운전 모드 이면,
if	Global_C_IO_Bit == L_Auto_Mode:
	# 1-1. 수동 운전 변수 초기화
	Global_C_Manual_Position_Bit=0
	Global_S_Manual_Action_Step=0

	# 1-2. 자동 운전 이벤트 확인    
	# Digital IO 4번 보드의 input 12 ~ 15번 ( 4 bit : Range 0 ~ 16 ) 비트를 읽어 자동 동작 위치 비트를 받는다.
	#2023.05.25 EDIT : MJ
	#Global_C_Auto_Action_Bit = get_digital_inputs(bit_start=L_CMD_BIT_1, bit_end=L_CMD_BIT_4)
	Global_C_Auto_Action_Bit = get_modbus_slave(133)
	# 명령 위치 확인
	# 명령 위치가 Loader A(2) 이면,
	if	Global_C_Auto_Action_Bit == L_Loader_A:
		# Loader A(2) 위치 값을 Global_C_Auto_Position 변수에 넣는다.
		Global_C_Auto_Position = Global_P_Loader_A
	# 명령 위치가 Loader B(3) 이면,
	elif	Global_C_Auto_Action_Bit == L_Loader_B:
		#Loader B(3) 위치 값을 Global_C_Auto_Position 변수에 넣는다.
		Global_C_Auto_Position = Global_P_Loader_B
	# 명령 위치가 Loader C(4) 이면,
	elif	Global_C_Auto_Action_Bit == L_Loader_C:
		#Loader C(4) 위치 값을 Global_C_Auto_Position 변수에 넣는다.
		Global_C_Auto_Position = Global_P_Loader_C
	# 명령 위치가 L_CooKer_1_1(5) 이면,
	elif	Global_C_Auto_Action_Bit == L_Cooker_1_1:
		#L_CooKer_1_1(5) 위치 값을 Global_C_Auto_Position 변수에 넣는다.
		Global_C_Auto_Position = Global_P_Cooker_Common
	# 명령 위치가 L_CooKer_1_2(6) 이면,
	elif	Global_C_Auto_Action_Bit == L_Cooker_1_2:
		#L_CooKer_1_2(6) 위치 값을 Global_C_Auto_Position 변수에 넣는다.
		Global_C_Auto_Position = Global_P_Cooker_Common
	# 명령 위치가 L_CooKer_2_1(7) 이면,
	elif	Global_C_Auto_Action_Bit == L_Cooker_2_1:
		# L_CooKer_2_1(7) 위치 값을 Global_C_Auto_Position 변수에 넣는다.
		Global_C_Auto_Position = Global_P_Cooker_Common
	# 명령 위치가 L_CooKer_2_2(8) 이면,
	elif	Global_C_Auto_Action_Bit == L_Cooker_2_2:
		# L_CooKer_2_2(8) 위치 값을 Global_C_Auto_Position 변수에 넣는다.
		Global_C_Auto_Position = Global_P_Cooker_Common
	# 명령 위치가 L_CooKer_3_1(9) 이면,
	elif	Global_C_Auto_Action_Bit == L_Cooker_3_1:
		# L_CooKer_3_1(9) 위치 값을 Global_C_Auto_Position 변수에 넣는다.
		Global_C_Auto_Position = Global_P_Cooker_Common
	# 명령 위치가 L_CooKer_3_2(10) 이면,
	elif	Global_C_Auto_Action_Bit == L_Cooker_3_2:
		#L_CooKer_3_2(10) 위치 값을 Global_C_Auto_Position 변수에 넣는다.
		Global_C_Auto_Position = Global_P_Cooker_3_2
	# 명령 위치가 L_Cooker_Function_A_Common(11) 이면,
	elif	Global_C_Auto_Action_Bit == L_Cooker_Function_A_Common:
		#L_Cooker_Function_A_Common(11) 위치 값을 Global_C_Auto_Position 변수에 넣는다.
		Global_C_Auto_Position = Global_P_Cooker_Common
	# 명령 위치가 L_Cooker_Function_A_3_2(12) 이면,
	elif	Global_C_Auto_Action_Bit == L_Cooker_Function_A_3_2:
		#L_Cooker_Function_A_3_2(12) 위치 값을 Global_C_Auto_Position 변수에 넣는다.
		Global_C_Auto_Position = Global_P_Cooker_3_2
	# 명령 위치가 L_Cooker_Function_B_Common(13) 이면,
	elif	Global_C_Auto_Action_Bit == L_Cooker_Function_B_Common:
		#L_Cooker_Function_B_Common(13) 위치 값을 Global_C_Auto_Position 변수에 넣는다.
		Global_C_Auto_Position = Global_P_Cooker_Common
	# 명령 위치가 L_Cooker_Function_B_3_2(14) 이면,
	elif	Global_C_Auto_Action_Bit == L_Cooker_Function_B_3_2:
		#L_Cooker_Function_B_3_2(14) 위치 값을 Global_C_Auto_Position 변수에 넣는다.
		Global_C_Auto_Position = Global_P_Cooker_3_2

	# 1-3. 자동 운전 프로세스 스텝을 업데이트
	#145 주소는 메뉴얼 운전 동작임으로 삭제 ::: 정명재
	#Global_M_Write_Data[145]=Global_S_Auto_Action_Step
	#모드 버스 145번지에 값을 적용 한다.
	#set_modbus_slave(145, Global_M_Write_Data[145])

	# 1-4. 자동 운전 명령 실행
	# 동작 명령 확인 비트가 0이 아니면,
	if	Global_C_Auto_Action_Bit!=0:
		# Step_00 : Step 업데이트
		# 자동 동작 프로세스 스텝이 0이면,
		if	Global_S_Auto_Action_Step==0:
			# 자동 동작 프로세스 스텝을 10으로 업데이트 한다.
			Global_S_Auto_Action_Step=10
		
		# Step_10 : 간섭 회피 위치 1 이동
		# 자동 동작 프로세스 스텝이 10이면 
		elif	Global_S_Auto_Action_Step==10:
			# Auto_Action_Bit가 Loader A(2)이면,
			if	Global_C_Auto_Action_Bit==L_Loader_A:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Loader_A_P1
			# Auto_Action_Bit가 Loader B(3)이면,
			elif	Global_C_Auto_Action_Bit==L_Loader_B:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Loader_B_P1
			# Auto_Action_Bit가 Loader C(4)이면,
			elif	Global_C_Auto_Action_Bit==L_Loader_C:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Loader_C_P1
			# Auto_Action_Bit가 Cooker 1-1, 1-2, 2-1, 2-2, 3-1, Function A Common, Function B Common(5 ~ 9, 11, 13)이면,
			elif	Global_C_Auto_Action_Bit==L_Cooker_1_1 or Global_C_Auto_Action_Bit==L_Cooker_1_2 or Global_C_Auto_Action_Bit==L_Cooker_2_1 or Global_C_Auto_Action_Bit==L_Cooker_2_2 or Global_C_Auto_Action_Bit==L_Cooker_3_1 or Global_C_Auto_Action_Bit==L_Cooker_Function_A_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Cooker_Common_P1
			# Auto_Action_Bit가 Cooker 3-2, Function A 3-2, Function B 3-2 (10, 12, 14)이면,
			elif	Global_C_Auto_Action_Bit==L_Cooker_3_2 or Global_C_Auto_Action_Bit==L_Cooker_Function_A_3_2 or Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Cooker_3_2_P1
			# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
			if	L_Target_Pos!=L_Interlock_Pos:
				# 이동 명령 상태 확인 변수 초기화
				Global_S_Auto_Check_Pos=-1
				# Loader A 투입 간섭 회피 위치 이동 명령 실행 후 결과 값을 리턴 한다
				Global_S_Auto_Check_Pos=movel(L_Target_Pos, vel=Global_C_Auto_Vel, acc=Global_C_Auto_Acc)
				# 이동 상태가 정상(0)일 경우에만 다음 Process 으로 넘긴다.
				if	Global_S_Auto_Check_Pos==0:
					# 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=11
				else:
					# 위치 이동 알람 활성화
					Global_S_Alarm_Pos_Move=1

		# Step_11 : 간섭 회피 위치 2 이동
		# 자동 동작 프로세스 스텝이 11이면,
		elif	Global_S_Auto_Action_Step==11:
			# Auto_Action_Bit가 Loader A(2)이면,
			if	Global_C_Auto_Action_Bit==L_Loader_A:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Loader_A_P2
			# Auto_Action_Bit가 Loader B(3)이면,
			elif	Global_C_Auto_Action_Bit==L_Loader_B:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Loader_B_P2
			# Auto_Action_Bit가 Loader C(4)이면,
			elif	Global_C_Auto_Action_Bit==L_Loader_C:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Loader_C_P2
			# Auto_Action_Bit가 Cooker 1-1, 1-2, 2-1, 2-2, 3-1, Function A Common, Function B Common(5 ~ 9, 11, 13)이면,	
			elif	Global_C_Auto_Action_Bit==L_Cooker_1_1 or Global_C_Auto_Action_Bit==L_Cooker_1_2 or Global_C_Auto_Action_Bit==L_Cooker_2_1 or Global_C_Auto_Action_Bit==L_Cooker_2_2 or Global_C_Auto_Action_Bit==L_Cooker_3_1 or Global_C_Auto_Action_Bit==L_Cooker_Function_A_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Cooker_Common_P2
			# Auto_Action_Bit가 Cooker 3-2, Function A 3-2, Function B 3-2 (10, 12, 14)이면,	
			elif Global_C_Auto_Action_Bit==L_Cooker_3_2 or Global_C_Auto_Action_Bit==L_Cooker_Function_A_3_2 or Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Cooker_3_2_P2
			# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
			if	L_Target_Pos!=L_Interlock_Pos:
				# 이동 명령 상태 확인 변수 초기화
				Global_S_Auto_Check_Pos=-1
				# Loader A 투입 간섭 회피 위치 이동 명령 실행 후 결과 값을 리턴 한다
				Global_S_Auto_Check_Pos=movel(L_Target_Pos, vel=Global_C_Auto_Vel, acc=Global_C_Auto_Acc)
				#	이동 상태가 정상(0)일 경우에만 다음 Process 으로 넘긴다.
				if	Global_S_Auto_Check_Pos==0:
					# 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=12
				else:
					# 위치 이동 알람 활성화
					Global_S_Alarm_Pos_Move=1

		# Step_12 : 간섭 회피 위치 3 이동
		# 자동 동작 프로세스 스텝이 12이면,
		elif	Global_S_Auto_Action_Step==12:
			# Auto_Action_Bit가 Loader A, Loader B, Loader C(2 ~ 9, 11, 13)이면,
			if	Global_C_Auto_Action_Bit==L_Loader_A or Global_C_Auto_Action_Bit==L_Loader_B or Global_C_Auto_Action_Bit==L_Loader_C or Global_C_Auto_Action_Bit==L_Cooker_1_1 or Global_C_Auto_Action_Bit==L_Cooker_1_2 or Global_C_Auto_Action_Bit==L_Cooker_2_1 or Global_C_Auto_Action_Bit==L_Cooker_2_2 or Global_C_Auto_Action_Bit==L_Cooker_3_1 or Global_C_Auto_Action_Bit==L_Cooker_Function_A_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common:
				# 다음 Process으로 넘기기 위해 자동 동작 프로세스 스텝을 업데이트
				Global_S_Auto_Action_Step=15
			# Auto_Action_Bit가 Cooker 3-2, Function A 3-2, Function B 3-2 (10, 12, 14)이면,
			elif	Global_C_Auto_Action_Bit==L_Cooker_3_2 or Global_C_Auto_Action_Bit==L_Cooker_Function_A_3_2 or Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Cooker_3_2_P2
			# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
			if	L_Target_Pos!=L_Interlock_Pos:
				# 이동 명령 상태 확인 변수 초기화
				Global_S_Auto_Check_Pos=-1
				# Loader A 투입 간섭 회피 위치 이동 명령 실행 후 결과 값을 리턴 한다
				Global_S_Auto_Check_Pos=movel(L_Target_Pos, vel=Global_C_Auto_Vel, acc=Global_C_Auto_Acc)
				# 이동 상태가 정상(0)일 경우에만 다음 Process 으로 넘긴다.
				if	Global_S_Auto_Check_Pos==0:
					# 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=15
				else:
					# 위치 이동 알람 활성화
					Global_S_Alarm_Pos_Move=1

		# Step_15 : 목표 위치 이동
		# 자동 동작 프로세스 스텝이 15이면,
		elif	Global_S_Auto_Action_Step==15:
			# 변수에 Target 위치 값을 넣는다.
			L_Target_Pos=Global_C_Auto_Position
			# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
			if	L_Target_Pos!=L_Interlock_Pos:
				# 이동 명령 상태 확인 변수 초기화
				Global_S_Auto_Check_Pos=-1
				# 이동 명령 실행 후 결과 값을 리턴 한다
				Global_S_Auto_Check_Pos=movel(L_Target_Pos, vel=Global_C_Auto_Vel, acc=Global_C_Auto_Acc)
				# 이동 상태가 정상(0)일 경우에만
				if	Global_S_Auto_Check_Pos==0:
					# 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=16
				else:
					# 위치 이동 알람 활성화
					Global_S_Alarm_Pos_Move=1

		# Step_16 : Gripper 상태 확인
		# 자동 동작 프로세스 스텝이 16이면,
		elif	Global_S_Auto_Action_Step==16:
			# Grip ON 상태이면, Grip OFF 명령 실행 ( A0509 )
			if	get_tool_digital_input(2)==ON and get_tool_digital_input(1)==OFF:
				# Grip On 상태으로 업데이트
				Global_S_Grip=1
				# Grip Off 명령 생성
				Global_C_Grip=2
				# 자동 동작 프로세스 스텝을 업데이트
				Global_S_Auto_Action_Step=20
			# Grip OFF 상태이면, Grip On 명령 실행 ( A0509 )
			elif	get_tool_digital_input(2)==OFF and get_tool_digital_input(1)==ON:
				# Grip Off 상태으로 업데이트
				Global_S_Grip=2
				# Grip On 명령 생성
				Global_C_Grip=1
				# 자동 동작 프로세스 스텝을 업데이트
				Global_S_Auto_Action_Step=20
			else:
				# Grip 상태 없음
				Global_S_Grip=0
				# Grip 센서 점검 필요
				Global_S_Auto_Check_Pos=3
				# Grip 상태가 없으므로 명령이 생성되면 안된다.
				Global_C_Grip=0
				# Grip 상태가 없으므로 자동 동작 프로세스 스텝을 업데이트
				Global_S_Auto_Action_Step=70
				# Grip 상태 알람 활성화
				Global_S_Alarm_Grip_Status=1

		# Step_20 : 자동운전 옵션 (Gripper 사용 유/무 확인)
		# 자동 동작 프로세스 스텝이 20이면,
		elif	Global_S_Auto_Action_Step==20:
			# Gripper 사용 모드 이면,
			if	Global_C_Gripper_Use==1 and Global_S_Grip==1:
				if	Global_C_Auto_Action_Bit==L_Cooker_1_1 or Global_C_Auto_Action_Bit==L_Cooker_1_2 or Global_C_Auto_Action_Bit==L_Cooker_2_1 or Global_C_Auto_Action_Bit==L_Cooker_2_2 or Global_C_Auto_Action_Bit==L_Cooker_3_1 or Global_C_Auto_Action_Bit==L_Cooker_3_2:
					# 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=21
				else:
					# 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=30
			# Grip 사용 모드 이고, 그립 상태가 아니면
			elif	Global_C_Gripper_Use==1 and Global_S_Grip==2:
				# 자동 동작 프로세스 스텝을 업데이트
				Global_S_Auto_Action_Step=30
			# Gripper 미 사용 모드 이면,
			elif	Global_C_Gripper_Use==0:
				# 자동 동작 프로세스 스텝을 업데이트
				Global_S_Auto_Action_Step=70

		# Step_21 : 자동운전 옵션 (투입 전 Basket Shake 사용 유/무 확인)
		# 자동 동작 프로세스 스텝이 21이면,
		elif	Global_S_Auto_Action_Step==21:
			# 투입 전 바스킷 흔들기 사용 모드 이면,
			if	Global_C_Basket_Shake==1:
				# 다음 Process으로 넘기기 위해 자동 동작 프로세스 스텝을 업데이트
				Global_S_Auto_Action_Step=22
			# 투입 전 바스킷 흔들기 미 사용 모드 이면,
			elif	Global_C_Basket_Shake==0:
				# 자동 동작 프로세스 스텝을 업데이트
				Global_S_Auto_Action_Step=30

		# Step_22 : 자동운전 옵션 (투입 전 Basket Shake 실행)
		# 자동 동작 프로세스 스텝이 22이면,
		elif	Global_S_Auto_Action_Step==22:
			# 반복 카운트 설정 ( Modbus로 받은 Recipe 값 )
			L_Basket_Shake_Cycle_Set_Count=Global_C_Basket_Shake_Count_A
			# 속도 설정 ( 고정 Recipe 값 )
			L_Basket_Shake_Vel=1000
			# 가속도 설정 ( 고정 Recipe 값 )
			L_Basket_Shake_Acc=1500
			# 설정 된 카운트가 될때 까지 반복
			for i in range(0, L_Basket_Shake_Cycle_Set_Count):
				# 모드 버스 143번지에서 읽은 Cobot 운전 속도 값 읽는다.
				Global_M_Read_Data[143]=get_modbus_slave(143)
				# Cobot 운전 속도 값이 0이 아닌 경우에만
				if	Global_M_Read_Data[143] != 0 and Global_M_Read_Data[143] != Global_C_Operation_Speed:
					# Cobot 속도 명령 변수 업데이트
					Global_C_Operation_Speed=Global_M_Read_Data[143]
					# Cobot 속도 변경 함수 실행
					change_operation_speed(Global_C_Operation_Speed)
					# Cobot 속도 상태 변수 업데이트
					Global_S_Operation_Speed=Global_C_Operation_Speed
					Global_M_Write_Data[159]=Global_S_Operation_Speed
					# 현재 속도 변경 값 쓰기
					set_modbus_slave(159, Global_M_Write_Data[159])
				# Cooker_3-2 위치 인 경우
				if	Global_C_Auto_Action_Bit==L_Cooker_3_2 or Global_C_Auto_Action_Bit==L_Cooker_Function_A_3_2:
					# Cooker3-2 바스킷 흔들기 위치 1 : 하강
					movel(Global_P_Cooker_3_2_S1, vel=L_Basket_Shake_Vel, acc=L_Basket_Shake_Acc)
					# Cooker3-2 바스킷 흔들기 위치 2 : 상승
					movel(Global_P_Cooker_3_2_S2, vel=L_Basket_Shake_Vel, acc=L_Basket_Shake_Acc)
					# Cooker3-2 바스킷 흔들기 위치 3 : 상승
					movel(Global_P_Cooker_3_2_S3, vel=L_Basket_Shake_Vel, acc=L_Basket_Shake_Acc)
				# Cooker_3-2 위치가 아닌 경우
				elif	Global_C_Auto_Action_Bit==L_Cooker_1_1 or Global_C_Auto_Action_Bit==L_Cooker_1_2 or Global_C_Auto_Action_Bit==L_Cooker_2_1 or Global_C_Auto_Action_Bit==L_Cooker_2_2 or Global_C_Auto_Action_Bit==L_Cooker_3_1 or Global_C_Auto_Action_Bit==L_Cooker_Function_A_Common:
					# Cooker 공통 바스킷 흔들기 위치 1 : 하강
					movel(Global_P_Cooker_Common_S1, vel=L_Basket_Shake_Vel, acc=L_Basket_Shake_Acc)
					# Cooker 공통 바스킷 흔들기 위치 2 : 상승
					movel(Global_P_Cooker_Common_S2, vel=L_Basket_Shake_Vel, acc=L_Basket_Shake_Acc)
					# Cooker 공통 바스킷 흔들기 위치 3 : 상승
					movel(Global_P_Cooker_Common_S3, vel=L_Basket_Shake_Vel, acc=L_Basket_Shake_Acc)
			# 투입 전 바스켓 흔들기가 완료 된 경우 모드버스에 값을 써준다
			Global_S_Basket_Shake_Count_A=Global_C_Basket_Shake_Count_A
			# 자동 동작 프로세스 스텝을 업데이트
			Global_S_Auto_Action_Step=23

		# Step_23 : 자동운전 옵션 (목표 위치 이동)
		# 자동 동작 프로세스 스텝이 23이면,
		elif	Global_S_Auto_Action_Step==23:
			# 변수에 Target 위치 값을 넣는다.
			L_Target_Pos=Global_C_Auto_Position
			# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
			if	L_Target_Pos!=L_Interlock_Pos:
				# 이동 명령 상태 확인 변수 초기화
				Global_S_Auto_Check_Pos=-1
				# 이동 명령 실행 후 결과 값을 리턴 한다
				Global_S_Auto_Check_Pos=movel(L_Target_Pos, vel=Global_C_Auto_Vel, acc=Global_C_Auto_Acc)
				# 이동 상태가 정상(0)일 경우에만
				if	Global_S_Auto_Check_Pos==0:
					# 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=30
				else:
					# 위치 이동 알람 활성화
					Global_S_Alarm_Pos_Move=1

		# Step_30 : Grip 상태 반전 명령
		# 자동 동작 프로세스 스텝이 30이면,
		elif	Global_S_Auto_Action_Step==30:
			# Grip On 명령이고, Grip ON 상태가 아니면, Grip On 명령 실행
			if	Global_C_Grip==1 and Global_S_Grip!=1 and Global_S_Check_Grip==0:
				# Grip 상태를 체크하기 위한 변수 값 -1 처리
				Global_S_Auto_Check_Pos=-1
				# Grip ON 명령 실행 ( A0509 )
				set_tool_digital_output(1,1)
				set_tool_digital_output(2,1)
				# Grip ON 명령 및 완료 상태 값 리턴 ( A0509 )
				Global_S_Check_Grip=wait_tool_digital_input(2, ON, 5)
				# Grip 센서가 상태가 정상(0)인 경우
				if	Global_S_Check_Grip==0:
					# Grip 상태 변수 Grip ON(1)으로 업데이트
					Global_S_Grip=1
					# 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=31
				# Grip 센서가 상태가 비 정상(음수 값)인 경우
				else:
					# Grip 상태 알람 활성화
					Global_S_Alarm_Grip_Status=1
			# Grip Off 명령이고, Grip off 상태가 아니면, Grip Off 명령 실행
			elif	Global_C_Grip==2 and Global_S_Grip!=2 and Global_S_Check_Grip==0:
				# Grip 상태를 체크하기 위한 변수 값 -1 처리
				Global_S_Check_Grip=-1
				#Grip OFF 명령 실행 ( A0509 )
				set_tool_digital_output(2,0)
				set_tool_digital_output(1,0)
				# Grip OFF 명령 및 완료 상태 값 리턴 ( A0509 )
				Global_S_Check_Grip=wait_tool_digital_input(1, ON, 5)
				# Grip 센서가 상태가 정상(0)인 경우
				if	Global_S_Check_Grip==0:
					# Grip 상태 변수 Grip OFF(2)으로 업데이트
					Global_S_Grip=2
					# 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=31
				# Grip 센서가 상태가 비 정상(음수 값)인 경우
				else:
					# Grip 상태 알람 활성화
					Global_S_Alarm_Grip_Status=1

		# Step_31 : Tool Weight 설정
		# 자동 동작 프로세스 스텝이 31이면,
		elif Global_S_Auto_Action_Step==31:
			#Grip ON 상태 이면,
			if	Global_S_Grip==1:
				# Basket_Weight : Basket + MQC + Gripper + JAW + 나사 = 2.8Kg
				if	Global_C_Tool_Weight==L_Tool_Weight_28:
					# Tool Weight 적용 상태를 체크하기 위한 변수 값 -1 처리
					Global_S_Check_Tool_Weight=-1
					# Tool Weight 적용 및 상태 값 리턴
					Global_S_Check_Tool_Weight = set_tool("Basket_Weight")
					# Tool Weight 적용 상태가 정상(0)이면,
					if	Global_S_Check_Tool_Weight==0:
						# 현재 적용된 Tool_Weight를 Basket_Weight 값으로 업데이트
						Global_S_Tool_Weight=L_Tool_Weight_28
						# 조리 중 Basket 흔들기 명령인 경우
						if	Global_C_Auto_Action_Bit==L_Cooker_Function_A_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_A_3_2:
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=35
						# 조리 중 설정(타이머) 시간만큼 Basket 들고 있기(산소 입히기) 명령인 경우
						elif	Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=40
						# 조리 중 Basket 흔들기/ 조리 중 설정(타이머) 시간만큼 Basket 들고 있기(산소 입히기) 명령이 아닌 경우
						elif	Global_C_Auto_Action_Bit==L_Loader_A or Global_C_Auto_Action_Bit==L_Loader_B or Global_C_Auto_Action_Bit==L_Loader_C or Global_C_Auto_Action_Bit==L_Cooker_1_1 or Global_C_Auto_Action_Bit==L_Cooker_1_2 or Global_C_Auto_Action_Bit==L_Cooker_2_1 or Global_C_Auto_Action_Bit==L_Cooker_2_2 or Global_C_Auto_Action_Bit==L_Cooker_3_1 or Global_C_Auto_Action_Bit==L_Cooker_3_2:
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=70
					# Tool Weight 적용 상태가 비 정상(음수)이면,
					else:
						# Grip 상태 알람 활성화
						Global_S_Alarm_Tool_Status=1
				# Bone05_Weight : 뼈 치킨 반마리 + Basket + MQC + Gripper + JAW + 나사 = 3.3Kg
				elif	Global_C_Tool_Weight==L_Tool_Weight_33:
					# Tool Weight 적용 상태를 체크하기 위한 변수 값 -1 처리
					Global_S_Check_Tool_Weight=-1
					# Tool Weight 적용 및 상태 값 리턴
					Global_S_Check_Tool_Weight=set_tool("Boneless05_Weight")
					# Tool Weight 적용 상태가 정상(0)이면,
					if	Global_S_Check_Tool_Weight==0:
						# 현재 적용된 Tool_Weight를 Boneless05_Weight 값으로 업데이트
						Global_S_Tool_Weight=L_Tool_Weight_33
						# 조리 중 Basket 흔들기 명령인 경우
						if	Global_C_Auto_Action_Bit==L_Cooker_Function_A_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_A_3_2:
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=35
						# 조리 중 설정(타이머) 시간만큼 Basket 들고 있기(산소 입히기) 명령인 경우
						elif Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=40
						# 조리 중 Basket 흔들기/ 조리 중 설정(타이머) 시간만큼 Basket 들고 있기(산소 입히기) 명령이 아닌 경우
						elif	Global_C_Auto_Action_Bit==L_Loader_A or Global_C_Auto_Action_Bit==L_Loader_B or Global_C_Auto_Action_Bit==L_Loader_C or Global_C_Auto_Action_Bit==L_Cooker_1_1 or Global_C_Auto_Action_Bit==L_Cooker_1_2 or Global_C_Auto_Action_Bit==L_Cooker_2_1 or Global_C_Auto_Action_Bit==L_Cooker_2_2 or Global_C_Auto_Action_Bit==L_Cooker_3_1 or Global_C_Auto_Action_Bit==L_Cooker_3_2:
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=70
					# Tool Weight 적용 상태가 비 정상(음수)이면,
					else:
						# Grip 상태 알람 활성화
						Global_S_Alarm_Tool_Status=1
				# Bone10_Weight : 뼈 치킨 한마리 + Basket + MQC + Gripper + JAW + 나사 = 3.8Kg
				elif	Global_C_Tool_Weight==L_Tool_Weight_38:
					# Tool Weight 적용 상태를 체크하기 위한 변수 값 -1 처리
					Global_S_Check_Tool_Weight=-1
					# Tool Weight 적용 및 상태 값 리턴
					Global_S_Check_Tool_Weight=set_tool("Boneless10_Weight")
					# Tool Weight 적용 상태가 정상(0)이면,
					if	Global_S_Check_Tool_Weight==0:
						# 현재 적용된 Tool_Weight를 Boneless10_Weight 값으로 업데이트
						Global_S_Tool_Weight = L_Tool_Weight_38
						# 조리 중 Basket 흔들기 명령인 경우
						if	Global_C_Auto_Action_Bit==L_Cooker_Function_A_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_A_3_2:
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=35
						# 조리 중 설정(타이머) 시간만큼 Basket 들고 있기(산소 입히기) 명령인 경우
						elif	Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=40
						# 조리 중 Basket 흔들기/ 조리 중 설정(타이머) 시간만큼 Basket 들고 있기(산소 입히기) 명령이 아닌 경우
						elif	Global_C_Auto_Action_Bit==L_Loader_A or Global_C_Auto_Action_Bit==L_Loader_B or Global_C_Auto_Action_Bit==L_Loader_C or Global_C_Auto_Action_Bit==L_Cooker_1_1 or Global_C_Auto_Action_Bit==L_Cooker_1_2 or Global_C_Auto_Action_Bit==L_Cooker_2_1 or Global_C_Auto_Action_Bit==L_Cooker_2_2 or Global_C_Auto_Action_Bit==L_Cooker_3_1 or Global_C_Auto_Action_Bit==L_Cooker_3_2:
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=70
					# Tool Weight 적용 상태가 비 정상(음수)이면,
					else:
						# Grip 상태 알람 활성화
						Global_S_Alarm_Tool_Status=1
				# Bone15_Weight : 뼈 치킨 한마리반 + Basket + MQC + Gripper + JAW + 나사 = 4.3Kg
				elif	Global_C_Tool_Weight==L_Tool_Weight_43:
					# Tool Weight 적용 상태를 체크하기 위한 변수 값 -1 처리
					Global_S_Check_Tool_Weight=-1
					# Tool Weight 적용 및 상태 값 리턴
					Global_S_Check_Tool_Weight=set_tool("Boneless15_Weight")
					# Tool Weight 적용 상태가 정상(0)이면,
					if	Global_S_Check_Tool_Weight==0:
						# 현재 적용된 Tool_Weight를 Boneless15_Weight 값으로 업데이트
						Global_S_Tool_Weight=L_Tool_Weight_43
						# 조리 중 Basket 흔들기 명령인 경우
						if	Global_C_Auto_Action_Bit==L_Cooker_Function_A_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_A_3_2:
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=35
						# 조리 중 설정(타이머) 시간만큼 Basket 들고 있기(산소 입히기) 명령인 경우
						elif	Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=40
						# 조리 중 Basket 흔들기/ 조리 중 설정(타이머) 시간만큼 Basket 들고 있기(산소 입히기) 명령이 아닌 경우
						elif	Global_C_Auto_Action_Bit==L_Loader_A or Global_C_Auto_Action_Bit==L_Loader_B or Global_C_Auto_Action_Bit==L_Loader_C or Global_C_Auto_Action_Bit==L_Cooker_1_1 or Global_C_Auto_Action_Bit==L_Cooker_1_2 or Global_C_Auto_Action_Bit==L_Cooker_2_1 or Global_C_Auto_Action_Bit==L_Cooker_2_2 or Global_C_Auto_Action_Bit==L_Cooker_3_1 or Global_C_Auto_Action_Bit==L_Cooker_3_2:
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=70
					# Tool Weight 적용 상태가 비 정상(음수)이면,
					else:
						# Grip 상태 알람 활성화
						Global_S_Alarm_Tool_Status=1
				# Boneless05_Weight : 순살 치킨 반마리 + Basket + MQC + Gripper + JAW + 나사 = 3.4Kg
				elif	Global_C_Tool_Weight==L_Tool_Weight_34:
					# Tool Weight 적용 상태를 체크하기 위한 변수 값 -1 처리
					Global_S_Check_Tool_Weight=-1
					# Tool Weight 적용 및 상태 값 리턴
					Global_S_Check_Tool_Weight=set_tool("Bone05_Weight")
					# Tool Weight 적용 상태가 정상(0)이면,
					if	Global_S_Check_Tool_Weight==0:
						# 현재 적용된 Tool_Weight를 Bone05_Weight 값으로 업데이트
						Global_S_Tool_Weight=L_Tool_Weight_34
						# 조리 중 Basket 흔들기 명령인 경우
						if	Global_C_Auto_Action_Bit==L_Cooker_Function_A_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_A_3_2:
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=35
						# 조리 중 설정(타이머) 시간만큼 Basket 들고 있기(산소 입히기) 명령인 경우
						elif	Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=40
						# 조리 중 Basket 흔들기/ 조리 중 설정(타이머) 시간만큼 Basket 들고 있기(산소 입히기) 명령이 아닌 경우
						elif	Global_C_Auto_Action_Bit==L_Loader_A or Global_C_Auto_Action_Bit==L_Loader_B or Global_C_Auto_Action_Bit==L_Loader_C or Global_C_Auto_Action_Bit==L_Cooker_1_1 or Global_C_Auto_Action_Bit==L_Cooker_1_2 or Global_C_Auto_Action_Bit==L_Cooker_2_1 or Global_C_Auto_Action_Bit==L_Cooker_2_2 or Global_C_Auto_Action_Bit==L_Cooker_3_1 or Global_C_Auto_Action_Bit==L_Cooker_3_2:
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=70
					# Tool Weight 적용 상태가 비 정상(음수)이면,
					else:
						# Grip 상태 알람 활성화
						Global_S_Alarm_Tool_Status=1

				# Boneless10_Weight : 순살 치킨 한마리 + Basket + MQC + Gripper + JAW + 나사 = 4.0Kg
				elif	Global_C_Tool_Weight==L_Tool_Weight_40:
					# Tool Weight 적용 상태를 체크하기 위한 변수 값 -1 처리
					Global_S_Check_Tool_Weight = -1
					# Tool Weight 적용 및 상태 값 리턴
					Global_S_Check_Tool_Weight = set_tool("Bone10_Weight")
					# Tool Weight 적용 상태가 정상(0)이면,
					if	Global_S_Check_Tool_Weight==0:
						# 현재 적용된 Tool_Weight를 Bone10_Weight 값으로 업데이트
						Global_S_Tool_Weight=L_Tool_Weight_40
						# 조리 중 Basket 흔들기 명령인 경우
						if	Global_C_Auto_Action_Bit==L_Cooker_Function_A_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_A_3_2:
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=35
						# 조리 중 설정(타이머) 시간만큼 Basket 들고 있기(산소 입히기) 명령인 경우
						elif	Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=40
						# 조리 중 Basket 흔들기/ 조리 중 설정(타이머) 시간만큼 Basket 들고 있기(산소 입히기) 명령이 아닌 경우
						elif	Global_C_Auto_Action_Bit==L_Loader_A or Global_C_Auto_Action_Bit==L_Loader_B or Global_C_Auto_Action_Bit==L_Loader_C or Global_C_Auto_Action_Bit==L_Cooker_1_1 or Global_C_Auto_Action_Bit==L_Cooker_1_2 or Global_C_Auto_Action_Bit==L_Cooker_2_1 or Global_C_Auto_Action_Bit==L_Cooker_2_2 or Global_C_Auto_Action_Bit==L_Cooker_3_1 or Global_C_Auto_Action_Bit==L_Cooker_3_2:
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=70
					# Tool Weight 적용 상태가 비 정상(음수)이면,
					else:
						# Grip 상태 알람 활성화
						Global_S_Alarm_Tool_Status=1

				# Boneless15_Weight : 순살 치킨 한마리반 + Basket + MQC + Gripper + JAW + 나사 = 4.6Kg
				elif	Global_C_Tool_Weight==L_Tool_Weight_46:
					# Tool Weight 적용 상태를 체크하기 위한 변수 값 -1 처리
					Global_S_Check_Tool_Weight = -1
					# Tool Weight 적용 및 상태 값 리턴
					Global_S_Check_Tool_Weight = set_tool("Bone15_Weight")
					# Tool Weight 적용 상태가 정상(0)이면,
					if	Global_S_Check_Tool_Weight == 0:
						# 현재 적용된 Tool_Weight를 Bone15_Weight 값으로 업데이트
						Global_S_Tool_Weight = L_Tool_Weight_46
						# 조리 중 Basket 흔들기 명령인 경우
						if	Global_C_Auto_Action_Bit==L_Cooker_Function_A_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_A_3_2:
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=35
						# 조리 중 설정(타이머) 시간만큼 Basket 들고 있기(산소 입히기) 명령인 경우
						elif	Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=40
						# 조리 중 Basket 흔들기/ 조리 중 설정(타이머) 시간만큼 Basket 들고 있기(산소 입히기) 명령이 아닌 경우
						elif	Global_C_Auto_Action_Bit==L_Loader_A or Global_C_Auto_Action_Bit==L_Loader_B or Global_C_Auto_Action_Bit==L_Loader_C or Global_C_Auto_Action_Bit==L_Cooker_1_1 or Global_C_Auto_Action_Bit==L_Cooker_1_2 or Global_C_Auto_Action_Bit==L_Cooker_2_1 or Global_C_Auto_Action_Bit==L_Cooker_2_2 or Global_C_Auto_Action_Bit==L_Cooker_3_1 or Global_C_Auto_Action_Bit==L_Cooker_3_2:
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=70
					# Tool Weight 적용 상태가 비 정상(음수)이면,
					else:
						# Grip 상태 알람 활성화
						Global_S_Alarm_Tool_Status=1

			# Grip OFF 상태이면, Gripper_Weight 값을 적용 한다.
			elif	Global_S_Grip == 2:
				# Tool Weight 적용 상태를 체크하기 위한 변수 값 -1 처리
				Global_S_Check_Tool_Weight=-1
				# Tool Weight 적용 및 상태 값 리턴
				Global_S_Check_Tool_Weight=set_tool("Gripper_Weight")
				# Tool Weight 적용 상태가 정상(0)이면,
				if	Global_S_Check_Tool_Weight==0:
					# 현재 적용된 Tool_Weight를 Gripper_Weight 값으로 업데이트
					Global_S_Tool_Weight=L_Tool_Weight_19
					# 조리 중 Basket 흔들기 명령인 경우
					if	Global_C_Auto_Action_Bit==L_Cooker_Function_A_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_A_3_2:
						# 자동 동작 프로세스 스텝을 업데이트
						Global_S_Auto_Action_Step=35
					# 조리 중 설정(타이머) 시간만큼 Basket 들고 있기(산소 입히기) 명령인 경우
					elif	Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
						# 자동 동작 프로세스 스텝을 업데이트
						Global_S_Auto_Action_Step=40
					# 조리 중 Basket 흔들기/ 조리 중 설정(타이머) 시간만큼 Basket 들고 있기(산소 입히기) 명령이 아닌 경우
					elif	Global_C_Auto_Action_Bit==L_Loader_A or Global_C_Auto_Action_Bit==L_Loader_B or Global_C_Auto_Action_Bit==L_Loader_C or Global_C_Auto_Action_Bit==L_Cooker_1_1 or Global_C_Auto_Action_Bit==L_Cooker_1_2 or Global_C_Auto_Action_Bit==L_Cooker_2_1 or Global_C_Auto_Action_Bit==L_Cooker_2_2 or Global_C_Auto_Action_Bit==L_Cooker_3_1 or Global_C_Auto_Action_Bit==L_Cooker_3_2:
						# 자동 동작 프로세스 스텝을 업데이트
						Global_S_Auto_Action_Step=70
				# Tool Weight 적용 상태가 비 정상(음수)이면,
				else:
					# Grip 상태 알람 활성화
					Global_S_Alarm_Tool_Status=1

		# Step_35 : 조리 중 Baske Shake
		# 자동 동작 프로세스 스텝이 35이면,
		elif	Global_S_Auto_Action_Step==35:
			# 현재 카운트 초기화
			L_Basket_Shake_Cycle_Current_Count=0
			# 반복 카운트 설정 ( Modbus로 받은 Recipe 값 )
			L_Basket_Shake_Cycle_Set_Count=Global_C_Basket_Shake_Count_B
			# 속도 설정 ( 고정 Recipe 값 )
			L_Basket_Shake_Vel=1000
			# 가속도 설정 ( 고정 Recipe 값 )
			L_Basket_Shake_Acc=1500
			# 설정 된 카운트가 될때 까지 반복
			for i in range(0, L_Basket_Shake_Cycle_Set_Count): 
				# 모드 버스 143번지에서 읽은 Cobot 운전 속도 값 읽는다.
				Global_M_Read_Data[143]=get_modbus_slave(143)
				# Cobot 운전 속도 값이 0이 아닌 경우에만
				if	Global_M_Read_Data[143] != 0 and Global_M_Read_Data[143] != Global_C_Operation_Speed:
					# Cobot 속도 명령 변수 업데이트
					Global_C_Operation_Speed=Global_M_Read_Data[143]
					# Cobot 속도 변경 함수 실행
					change_operation_speed(Global_C_Operation_Speed)
					# Cobot 속도 상태 변수 업데이트
					Global_S_Operation_Speed=Global_C_Operation_Speed
					Global_M_Write_Data[159]=Global_S_Operation_Speed
					# 현재 속도 변경 값 쓰기
					set_modbus_slave(159, Global_M_Write_Data[159])
				# Cooker_3-2 위치 인 경우
				if	Global_C_Auto_Action_Bit==L_Cooker_3_2 or Global_C_Auto_Action_Bit==L_Cooker_Function_A_3_2 or Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
					# Cooker3-2 바스킷 흔들기 위치 1 : 하강
					movel(Global_P_Cooker_3_2_S1, vel=L_Basket_Shake_Vel, acc=L_Basket_Shake_Acc)
					# Cooker3-2 바스킷 흔들기 위치 2 : 상승
					movel(Global_P_Cooker_3_2_S2, vel=L_Basket_Shake_Vel, acc=L_Basket_Shake_Acc)
					# Cooker3-2 바스킷 흔들기 위치 3 : 상승
					movel(Global_P_Cooker_3_2_S3, vel=L_Basket_Shake_Vel, acc=L_Basket_Shake_Acc)
				# Cooker_3-2 위치가 아닌 경우
				elif	Global_C_Auto_Action_Bit==L_Cooker_1_1 or Global_C_Auto_Action_Bit==L_Cooker_1_2 or Global_C_Auto_Action_Bit==L_Cooker_2_1 or Global_C_Auto_Action_Bit==L_Cooker_2_2 or Global_C_Auto_Action_Bit==L_Cooker_3_1 or Global_C_Auto_Action_Bit==L_Cooker_Function_A_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common:
					# Cooker 공통 바스킷 흔들기 위치 1 : 하강
					movel(Global_P_Cooker_Common_S1, vel=L_Basket_Shake_Vel, acc=L_Basket_Shake_Acc)
					# Cooker 공통 바스킷 흔들기 위치 2 : 상승
					movel(Global_P_Cooker_Common_S2, vel=L_Basket_Shake_Vel, acc=L_Basket_Shake_Acc)
					# Cooker 공통 바스킷 흔들기 위치 3 : 상승
					movel(Global_P_Cooker_Common_S3, vel=L_Basket_Shake_Vel, acc=L_Basket_Shake_Acc)
			# 조리 중 바스켓 흔들기가 완료 된 경우 모드버스에 값을 써준다
			Global_S_Basket_Shake_Count_B=Global_C_Basket_Shake_Count_B
			# 자동 동작 프로세스 스텝을 업데이트
			Global_S_Auto_Action_Step=36

		# Step_36 : 투입 목표 위치 이동
		# 자동 동작 프로세스 스텝이 36이면,
		elif	Global_S_Auto_Action_Step==36:
			# Auto_Action_Bit가 조리 중 Baske Shake(11, 12) 위치 이면,
			if Global_C_Auto_Action_Bit==L_Cooker_Function_A_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_A_3_2:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_C_Auto_Position
				# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
				if	L_Target_Pos!=L_Interlock_Pos:
					# 이동 명령 상태 확인 변수 초기화
					Global_S_Auto_Check_Pos=-1
					# Cooker_3-2 위치기 아닌 경우
					Global_S_Auto_Check_Pos=movel(Global_C_Auto_Position, vel=Global_C_Auto_Vel, acc=Global_C_Auto_Acc)
					# 이동 상태가 정상(0)일 경우에만
					if	Global_S_Auto_Check_Pos==0:
						# 자동 동작 프로세스 스텝을 업데이트
						Global_S_Auto_Action_Step=60
					else:
						# 위치 이동 알람 활성화
						Global_S_Alarm_Pos_Move=1

		# Step_40 : 간섭 회피 위치 3 이동
		# 자동 동작 프로세스 스텝이 40이면,
		elif	Global_S_Auto_Action_Step==40:
			# Auto_Action_Bit가 조리 중 설정(타이머) 시간만큼 들고 있기 명령 Cooker 공통 위치(13) 이면,
			if	Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Cooker_Common_P2
			# Auto_Action_Bit가 조리 중 설정(타이머) 시간만큼 들고 있기 명령 Cooker 3-2 위치(14) 이면,
			elif	Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Cooker_3_2_P3
			# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
			if	L_Target_Pos!=L_Interlock_Pos:
				# 이동 명령 상태 확인 변수 초기화
				Global_S_Auto_Check_Pos=-1
				# Loader A 투입 간섭 회피 위치 이동 명령 실행 후 결과 값을 리턴 한다
				Global_S_Auto_Check_Pos=movel(L_Target_Pos, vel=Global_C_Auto_Vel, acc=Global_C_Auto_Acc)
				# 이동 상태가 정상(0)일 경우에만 다음 Process 으로 넘긴다.
				if	Global_S_Auto_Check_Pos==0:
					# 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=41
				else:
					# 위치 이동 알람 활성화
					Global_S_Alarm_Pos_Move=1 

		# Step_41 : 간섭 회피 위치 2 이동
		# 자동 동작 프로세스 스텝이 41이면,
		elif	Global_S_Auto_Action_Step==41:
			# Auto_Action_Bit가 조리 중 설정(타이머) 시간만큼 들고 있기 명령 Cooker 공통 위치(13) 이면,
			if	Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Cooker_Common_P1
			# Auto_Action_Bit가 조리 중 설정(타이머) 시간만큼 들고 있기 명령 Cooker 3-2 위치(14) 이면,
			elif	Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Cooker_3_2_P2
			# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
			if	L_Target_Pos!=L_Interlock_Pos:
				# 이동 명령 상태 확인 변수 초기화
				Global_S_Auto_Check_Pos=-1
				# Loader A 투입 간섭 회피 위치 이동 명령 실행 후 결과 값을 리턴 한다
				Global_S_Auto_Check_Pos=movel(L_Target_Pos, vel=Global_C_Auto_Vel, acc=Global_C_Auto_Acc)
				# 이동 상태가 정상(0)일 경우에만 다음 Process 으로 넘긴다.
				if	Global_S_Auto_Check_Pos==0:
					# 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=42
				else:
					# 위치 이동 알람 활성화
					Global_S_Alarm_Pos_Move=1

		# Step_42 : 간섭 회피 위치 1 이동
		# 자동 동작 프로세스 스텝이 42이면,
		elif	Global_S_Auto_Action_Step==42:
			#	Auto_Action_Bit가 조리 중 설정(타이머) 시간만큼 들고 있기 명령 Cooker 공통 위치(13) 이면,
			if	Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Wait
			# Auto_Action_Bit가 조리 중 설정(타이머) 시간만큼 들고 있기 명령 Cooker 3-2 위치(14) 이면,
			elif	Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Cooker_3_2_P1
			# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
			if	L_Target_Pos!=L_Interlock_Pos:
				# 이동 명령 상태 확인 변수 초기화
				Global_S_Auto_Check_Pos=-1
				# Loader A 투입 간섭 회피 위치 이동 명령 실행 후 결과 값을 리턴 한다
				Global_S_Auto_Check_Pos=movel(L_Target_Pos, vel=Global_C_Auto_Vel, acc=Global_C_Auto_Acc)
				# 이동 상태가 정상(0)일 경우에만 다음 Process 으로 넘긴다.
				if	Global_S_Auto_Check_Pos==0:
					# 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=45 
				else:
					# 위치 이동 알람 활성화
					Global_S_Alarm_Pos_Move=1 

		# Step_45 : 설정 시간(타이머)동안 들고 있기 (산소 입히기)
		# 자동 동작 프로세스 스텝이 45이면,
		elif	Global_S_Auto_Action_Step==45:
			# 시간 설정 변수 초기화
			L_Set_Timer=0
			# 시간 설정 ( Modbus로 받은 Recipe 값 )
			L_Set_Timer=Global_C_Timer
			# Auto_Action_Bit가 조리 중 설정(타이머) 시간만큼 들고 있기(13~14) 명령 이면,
			if	Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
				# 이동 명령 상태 확인 변수 초기화
				Global_S_Auto_Check_Pos=-1
				# 설정된 시간 만큼 대기 후 결과 값을 리턴 한다
				Global_S_Auto_Check_Pos=wait(L_Set_Timer)
				# 이동 상태가 정상(0)일 경우에만 다음 Process 으로 넘긴다.
				if	Global_S_Auto_Check_Pos==0:
					# 설정 된 타이머 만큼 시간이 경과 한 경우 모드버스에 써준다.
					Global_S_Timer=Global_C_Timer
					# 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=50
				else:
					# 대기 시간 알람 활성화
					Global_S_Alarm_Timeout=1 

		# Step_50 : 간섭 회피 위치 1 이동
		# 자동 동작 프로세스 스텝이 50이면,
		elif Global_S_Auto_Action_Step==50:
			# Auto_Action_Bit가 조리 중 설정(타이머) 시간만큼 들고 있기 명령 Cooker 공통 위치(13) 이면,
			if	Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Cooker_Common_P1
			# Auto_Action_Bit가 조리 중 설정(타이머) 시간만큼 들고 있기 명령 Cooker 3-2 위치(14) 이면,
			elif Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Cooker_3_2_P1
			# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
			if	L_Target_Pos!=L_Interlock_Pos:
				# 이동 명령 상태 확인 변수 초기화
				Global_S_Auto_Check_Pos=-1
				# Loader A 투입 간섭 회피 위치 이동 명령 실행 후 결과 값을 리턴 한다
				Global_S_Auto_Check_Pos=movel(L_Target_Pos, vel=Global_C_Auto_Vel, acc=Global_C_Auto_Acc)
				# 이동 상태가 정상(0)일 경우에만 다음 Process 으로 넘긴다.
				if	Global_S_Auto_Check_Pos==0:
					# 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=51
				else:
					# 위치 이동 알람 활성화
					Global_S_Alarm_Pos_Move=1 

		# Step_51 : 간섭 회피 위치 2 이동
		# 자동 동작 프로세스 스텝이 51이면,
		elif	Global_S_Auto_Action_Step==51:
			# Auto_Action_Bit가 조리 중 설정(타이머) 시간만큼 들고 있기 명령 Cooker 공통 위치(13) 이면,
			if	Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Cooker_Common_P2
			# Auto_Action_Bit가 조리 중 설정(타이머) 시간만큼 들고 있기 명령 Cooker 3-2 위치(14) 이면,
			elif	Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Cooker_3_2_P2
			# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
			if	L_Target_Pos!=L_Interlock_Pos:
				# 이동 명령 상태 확인 변수 초기화
				Global_S_Auto_Check_Pos=-1
				# Loader A 투입 간섭 회피 위치 이동 명령 실행 후 결과 값을 리턴 한다
				Global_S_Auto_Check_Pos=movel(L_Target_Pos, vel=Global_C_Auto_Vel, acc=Global_C_Auto_Acc)
				# 이동 상태가 정상(0)일 경우에만 다음 Process 으로 넘긴다.
				if	Global_S_Auto_Check_Pos==0:
					# 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=52
				else:
					# 위치 이동 알람 활성화
					Global_S_Alarm_Pos_Move=1 

		# Step_52 : 간섭 회피 위치 3 이동 
		# 자동 동작 프로세스 스텝이 52이면,
		elif	Global_S_Auto_Action_Step==52:
			# Auto_Action_Bit가 조리 중 설정(타이머) 시간만큼 들고 있기 명령 Cooker 공통 위치(13) 이면,
			if	Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common:
				# 자동 동작 프로세스 스텝을 업데이트 ( 간섭 회피 위치 이동 3번이 없어서 넘긴다 )
				Global_S_Auto_Action_Step=55
			# Auto_Action_Bit가 조리 중 설정(타이머) 시간만큼 들고 있기 명령 Cooker 3-2 위치(14) 이면,
			elif	Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Cooker_3_2_P3
			# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
			if	L_Target_Pos!=L_Interlock_Pos:
				# 이동 명령 상태 확인 변수 초기화	
				Global_S_Auto_Check_Pos=-1
				# Loader A 투입 간섭 회피 위치 이동 명령 실행 후 결과 값을 리턴 한다
				Global_S_Auto_Check_Pos=movel(L_Target_Pos, vel=Global_C_Auto_Vel, acc=Global_C_Auto_Acc)
				# 이동 상태가 정상(0)일 경우에만 다음 Process 으로 넘긴다.
				if	Global_S_Auto_Check_Pos==0:
					# 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=55
				else:
					# 위치 이동 알람 활성화
					Global_S_Alarm_Pos_Move=1 

		# Step_55 : 투입 목표 위치 이동
		# 자동 동작 프로세스 스텝이 55이면,
		elif	Global_S_Auto_Action_Step==55:
			# Auto_Action_Bit가 조리 중 설정(타이머) 시간만큼 들고 있기 (13, 14) 이면,
			if	Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_C_Auto_Position
				# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
				if	L_Target_Pos!=L_Interlock_Pos:
					# 이동 명령 상태 확인 변수 초기화
					Global_S_Auto_Check_Pos=-1
					# Cooker_3-2 위치기 아닌 경우
					Global_S_Auto_Check_Pos=movel(L_Target_Pos, vel=Global_C_Auto_Vel, acc=Global_C_Auto_Acc)
					# 이동 상태가 정상(0)일 경우에만
					if	Global_S_Auto_Check_Pos==0:
						# 자동 동작 프로세스 스텝을 업데이트
						Global_S_Auto_Action_Step=60
					else:
						# 위치 이동 알람 활성화
						Global_S_Alarm_Pos_Move=1  

		# Step_60 : Gripper 상태 확인
		# 자동 동작 프로세스 스텝이 60이면,
		elif	Global_S_Auto_Action_Step==60:
			# Auto_Action_Bit가 조리중 Basket 흔들기 / 조리 중 설정(타이머) 시간만큼 들고 있기 (11 ~ 14) 이면
			if	Global_C_Auto_Action_Bit==L_Cooker_Function_A_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_A_3_2 or Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
				#Grip ON 상태이면, Grip OFF 명령 실행 ( A0509 )
				if	get_tool_digital_input(2)==ON and get_tool_digital_input(1)==OFF:
					# Grip On 상태으로 업데이트
					Global_S_Grip=1
					# Grip Off 명령 생성
					Global_C_Grip=2	
					# 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=61
				# Grip OFF 상태이면, Grip On 명령 실행 ( A0509 )
				elif	get_tool_digital_input(2)==OFF and get_tool_digital_input(1)==ON:
					# Grip Off 상태으로 업데이트
					Global_S_Grip=2
					# Grip On 명령 생성
					Global_C_Grip=1
					# 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=61
				# Grip 상태가 On 또는 Off 상태가 아니면,
				else:
					# Grip 상태 없음
					Global_S_Grip=0
					# Grip 센서 점검 필요
					Global_S_Auto_Check_Pos=3
					# Grip 상태가 없으므로 명령이 생성되면 안된다.
					Global_C_Grip=0
					# Grip 상태가 없으므로 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=70
					# Grip 상태 알람 활성화
					Global_S_Alarm_Grip_Status=1

		# Step_61 : Grip 상태 반전 명령
		# 자동 동작 프로세스 스텝이 61이면,
		elif	Global_S_Auto_Action_Step==61:
			# Auto_Action_Bit가 조리중 Basket 흔들기 / 조리 중 설정(타이머) 시간만큼 들고 있기 (11 ~ 14) 이면
			if	Global_C_Auto_Action_Bit==L_Cooker_Function_A_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_A_3_2 or Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
				# Grip On 명령이고, Grip ON 상태가 아니면, Grip On 명령 실행
				if	Global_C_Grip==1 and Global_S_Grip!=1 and Global_S_Check_Grip==0:
					# Grip 상태를 체크하기 위한 변수 값 -1 처리
					Global_S_Auto_Check_Pos=-1
					# Grip ON 명령 실행 ( A0509 )
					set_tool_digital_output(1,1)
					set_tool_digital_output(2,1)
					# Grip ON 명령 및 완료 상태 값 리턴 ( A0509 )
					Global_S_Check_Grip=wait_tool_digital_input(2, ON, 5)
					# Grip 센서가 상태가 정상(0)인 경우
					if	Global_S_Check_Grip==0:
						# Grip 상태 변수 Grip ON(1)으로 업데이트
						Global_S_Grip=1
						# 자동 동작 프로세스 스텝을 업데이트
						Global_S_Auto_Action_Step=62
					# Grip 센서가 상태가 비 정상(음수)인 경우
					else:
						# Grip 상태 알람 활성화
						Global_S_Alarm_Grip_Status=1
				# Grip Off 명령이고, Grip off 상태가 아니면, Grip Off 명령 실행
				elif	Global_C_Grip==2 and Global_S_Grip!=2 and Global_S_Check_Grip==0:
					# Grip 상태를 체크하기 위한 변수 값 -1 처리
					Global_S_Check_Grip=-1
					#Grip OFF 명령 실행 ( A0509 )
					set_tool_digital_output(2,0)
					set_tool_digital_output(1,0)
					# Grip OFF 명령 및 완료 상태 값 리턴 ( A0509 )
					Global_S_Check_Grip=wait_tool_digital_input(1, ON, 5)
					# Grip 센서가 상태가 정상(0)인 경우
					if	Global_S_Check_Grip==0:
						# Grip 상태 변수 Grip OFF(2)으로 업데이트
						Global_S_Grip=2
						# 자동 동작 프로세스 스텝을 업데이트
						Global_S_Auto_Action_Step=62
					# Grip 센서가 상태가 비 정상(음수)인 경우
					else:
						# Grip 상태 알람 활성화
						Global_S_Alarm_Grip_Status=1

		# Step_62 : Tool Weight 설정
		# 자동 동작 프로세스 스텝이 62이면,
		elif	Global_S_Auto_Action_Step==62:
			# 조리 중 Basket 흔들기 / 조리 중 설정(타이머) 시간만큼 Basket 들고 있기(산소 입히기) 명령인 경우
			if	Global_C_Auto_Action_Bit==L_Cooker_Function_A_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_A_3_2 or Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
				#Grip ON 상태 이면,
				if	Global_S_Grip==1:
					# Basket_Weight : Basket + MQC + Gripper + JAW + 나사 = 2.8Kg
					if	Global_C_Tool_Weight==L_Tool_Weight_28:
						# Tool Weight 적용 상태를 체크하기 위한 변수 값 -1 처리
						Global_S_Check_Tool_Weight=-1
						# Tool Weight 적용 및 상태 값 리턴
						Global_S_Check_Tool_Weight = set_tool("Basket_Weight")
						# Tool Weight 적용 상태가 정상(0)이면,
						if	Global_S_Check_Tool_Weight==0:
							# 현재 적용된 Tool_Weight를 Basket_Weight 값으로 업데이트
							Global_S_Tool_Weight=L_Tool_Weight_28
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=70
						# Tool Weight 적용 상태가 비 정상(음수)이면,
						else:
							# Grip 상태 알람 활성화
							Global_S_Alarm_Tool_Status=1
					# Bone05_Weight : 뼈 치킨 반마리 + Basket + MQC + Gripper + JAW + 나사 = 3.3Kg
					elif	Global_C_Tool_Weight==L_Tool_Weight_33:
						# Tool Weight 적용 상태를 체크하기 위한 변수 값 -1 처리
						Global_S_Check_Tool_Weight=-1
						# Tool Weight 적용 및 상태 값 리턴
						Global_S_Check_Tool_Weight=set_tool("Boneless05_Weight")
						# Tool Weight 적용 상태가 정상(0)이면,
						if	Global_S_Check_Tool_Weight==0:
							# 현재 적용된 Tool_Weight를 Boneless05_Weight 값으로 업데이트
							Global_S_Tool_Weight=L_Tool_Weight_33
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=70
						# Tool Weight 적용 상태가 비 정상(음수)이면,
						else:
							# Grip 상태 알람 활성화
							Global_S_Alarm_Tool_Status=1
					# Bone10_Weight : 뼈 치킨 한마리 + Basket + MQC + Gripper + JAW + 나사 = 3.8Kg
					elif	Global_C_Tool_Weight==L_Tool_Weight_38:
						# Tool Weight 적용 상태를 체크하기 위한 변수 값 -1 처리
						Global_S_Check_Tool_Weight=-1
						# Tool Weight 적용 및 상태 값 리턴
						Global_S_Check_Tool_Weight=set_tool("Boneless10_Weight")
						# Tool Weight 적용 상태가 정상(0)이면,
						if	Global_S_Check_Tool_Weight==0:
							# 현재 적용된 Tool_Weight를 Boneless10_Weight 값으로 업데이트
							Global_S_Tool_Weight = L_Tool_Weight_38
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=70
						# Tool Weight 적용 상태가 비 정상(음수)이면,
						else:
							# Grip 상태 알람 활성화
							Global_S_Alarm_Tool_Status=1
					# Bone15_Weight : 뼈 치킨 한마리반 + Basket + MQC + Gripper + JAW + 나사 = 4.3Kg
					elif	Global_C_Tool_Weight==L_Tool_Weight_43:
						# Tool Weight 적용 상태를 체크하기 위한 변수 값 -1 처리
						Global_S_Check_Tool_Weight=-1
						# Tool Weight 적용 및 상태 값 리턴
						Global_S_Check_Tool_Weight=set_tool("Boneless15_Weight")
						# Tool Weight 적용 상태가 정상(0)이면,
						if	Global_S_Check_Tool_Weight==0:
							# 현재 적용된 Tool_Weight를 Boneless15_Weight 값으로 업데이트
							Global_S_Tool_Weight=L_Tool_Weight_43
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Ation_Step=70
						else:
							# Grip 상태 알람 활성화
							Global_S_Alarm_Tool_Status=1
					# Boneless05_Weight : 순살 치킨 반마리 + Basket + MQC + Gripper + JAW + 나사 = 3.4Kg
					elif	Global_C_Tool_Weight==L_Tool_Weight_34:
						# Tool Weight 적용 상태를 체크하기 위한 변수 값 -1 처리
						Global_S_Check_Tool_Weight=-1
						# Tool Weight 적용 및 상태 값 리턴
						Global_S_Check_Tool_Weight=set_tool("Bone05_Weight")
						# Tool Weight 적용 상태가 정상(0)이면,
						if	Global_S_Check_Tool_Weight==0:
							# 현재 적용된 Tool_Weight를 Bone05_Weight 값으로 업데이트
							Global_S_Tool_Weight=L_Tool_Weight_34
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=70
						else:
							# Grip 상태 알람 활성화
							Global_S_Alarm_Tool_Status=1
					# Boneless10_Weight : 순살 치킨 한마리 + Basket + MQC + Gripper + JAW + 나사 = 4.0Kg
					elif	Global_C_Tool_Weight==L_Tool_Weight_40:
						# Tool Weight 적용 상태를 체크하기 위한 변수 값 -1 처리
						Global_S_Check_Tool_Weight = -1
						# Tool Weight 적용 및 상태 값 리턴
						Global_S_Check_Tool_Weight = set_tool("Bone10_Weight")
						# Tool Weight 적용 상태가 정상(0)이면,
						if	Global_S_Check_Tool_Weight==0:
							# 현재 적용된 Tool_Weight를 Bone10_Weight 값으로 업데이트
							Global_S_Tool_Weight=L_Tool_Weight_40
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=70
						else:
							# Grip 상태 알람 활성화
							Global_S_Alarm_Tool_Status=1
					# Boneless15_Weight : 순살 치킨 한마리반 + Basket + MQC + Gripper + JAW + 나사 = 4.6Kg
					elif	Global_C_Tool_Weight==L_Tool_Weight_46:
						# Tool Weight 적용 상태를 체크하기 위한 변수 값 -1 처리
						Global_S_Check_Tool_Weight = -1
						# Tool Weight 적용 및 상태 값 리턴
						Global_S_Check_Tool_Weight = set_tool("Bone15_Weight")
						# Tool Weight 적용 상태가 정상(0)이면,
						if	Global_S_Check_Tool_Weight == 0:
							# 현재 적용된 Tool_Weight를 Bone15_Weight 값으로 업데이트
							Global_S_Tool_Weight = L_Tool_Weight_46
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=70
						else:
							# Grip 상태 알람 활성화
							Global_S_Alarm_Tool_Status=1
				# Grip OFF 상태이면, Gripper_Weight 값을 적용 한다.
				elif	Global_S_Grip == 2:
					# Tool Weight 적용 상태를 체크하기 위한 변수 값 -1 처리
					Global_S_Check_Tool_Weight=-1
					# Tool Weight 적용 및 상태 값 리턴
					Global_S_Check_Tool_Weight=set_tool("Gripper_Weight")
					# Tool Weight 적용 상태가 정상(0)이면,
					if	Global_S_Check_Tool_Weight==0:
						# 현재 적용된 Tool_Weight를 Gripper_Weight 값으로 업데이트
						Global_S_Tool_Weight=L_Tool_Weight_19
						# 자동 동작 프로세스 스텝을 업데이트
						Global_S_Auto_Action_Step=70
					else:
						# Grip 상태 알람 활성화
						Global_S_Alarm_Tool_Status=1

		# Step_70 : 간섭 회피 위치 3 이동
		# 자동 동작 프로세스 스텝이 70이면,
		elif	Global_S_Auto_Action_Step==70:
			# Auto_Action_Bit가 Loader A, Loader B, Loader C(2 ~ 9, 11, 13)이면,
			if	Global_C_Auto_Action_Bit==L_Loader_A or Global_C_Auto_Action_Bit==L_Loader_B or Global_C_Auto_Action_Bit==L_Loader_C or Global_C_Auto_Action_Bit==L_Cooker_1_1 or Global_C_Auto_Action_Bit==L_Cooker_1_2 or Global_C_Auto_Action_Bit==L_Cooker_2_1 or Global_C_Auto_Action_Bit==L_Cooker_2_2 or Global_C_Auto_Action_Bit==L_Cooker_3_1 or Global_C_Auto_Action_Bit==L_Cooker_Function_A_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common:
				# 다음 Process으로 넘기기 위해 자동 동작 프로세스 스텝을 업데이트
				Global_S_Auto_Action_Step=71
			# Auto_Action_Bit가 Cooker 3-2, Function A 3-2, Function B 3-2 (10, 12, 14)이면,
			elif	Global_C_Auto_Action_Bit==L_Cooker_3_2 or Global_C_Auto_Action_Bit==L_Cooker_Function_A_3_2 or Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Cooker_3_2_P2
			# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
			if	L_Target_Pos!=L_Interlock_Pos:
				# 이동 명령 상태 확인 변수 초기화
				Global_S_Auto_Check_Pos=-1
				# Loader A 투입 간섭 회피 위치 이동 명령 실행 후 결과 값을 리턴 한다
				Global_S_Auto_Check_Pos=movel(L_Target_Pos, vel=Global_C_Auto_Vel, acc=Global_C_Auto_Acc)	
				# 이동 상태가 정상(0)일 경우에만 다음 Process 으로 넘긴다.
				if	Global_S_Auto_Check_Pos==0:
					# 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=71
				else:
					# 위치 이동 알람 활성화
					Global_S_Alarm_Pos_Move=1  

		# Step_71 : 간섭 회피 위치 2 이동
		# 자동 동작 프로세스 스텝이 71이면,
		elif	Global_S_Auto_Action_Step==71:
			# Auto_Action_Bit가 Loader A(2)이면,
			if	Global_C_Auto_Action_Bit==L_Loader_A:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Loader_A_P2
			# Auto_Action_Bit가 Loader B(3)이면,
			elif	Global_C_Auto_Action_Bit==L_Loader_B:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Loader_B_P2
			# Auto_Action_Bit가 Loader C(4)이면,
			elif	Global_C_Auto_Action_Bit==L_Loader_C:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Loader_C_P2
			# Auto_Action_Bit가 Cooker 1-1, 1-2, 2-1, 2-2, 3-1, Function A Common, Function B Common(5 ~ 9, 11, 13)이면,
			elif	Global_C_Auto_Action_Bit==L_Cooker_1_1 or Global_C_Auto_Action_Bit==L_Cooker_1_2 or Global_C_Auto_Action_Bit==L_Cooker_2_1 or Global_C_Auto_Action_Bit==L_Cooker_2_2 or Global_C_Auto_Action_Bit==L_Cooker_3_1 or Global_C_Auto_Action_Bit==L_Cooker_Function_A_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Cooker_Common_P2
			# Auto_Action_Bit가 Cooker 3-2, Function A 3-2, Function B 3-2 (10, 12, 14)이면,	
			elif	Global_C_Auto_Action_Bit==L_Cooker_3_2 or Global_C_Auto_Action_Bit==L_Cooker_Function_A_3_2 or Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Cooker_3_2_P2
			# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
			if	L_Target_Pos!=L_Interlock_Pos:
				# 이동 명령 상태 확인 변수 초기화
				Global_S_Auto_Check_Pos=-1
				# Loader A 투입 간섭 회피 위치 이동 명령 실행 후 결과 값을 리턴 한다
				Global_S_Auto_Check_Pos=movel(L_Target_Pos, vel=Global_C_Auto_Vel, acc=Global_C_Auto_Acc)
				# 이동 상태가 정상(0)일 경우에만 다음 Process 으로 넘긴다.
				if	Global_S_Auto_Check_Pos==0:
					# 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=72
				else:
					# 위치 이동 알람 활성화
					Global_S_Alarm_Pos_Move=1 

		# Step_72 : 간섭 회피 위치 1 이동
		# 자동 동작 프로세스 스텝이 72이면,
		elif	Global_S_Auto_Action_Step==72:
			# Auto_Action_Bit가 Loader A(2)이면,
			if	Global_C_Auto_Action_Bit==L_Loader_A:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Loader_A_P1
			# Auto_Action_Bit가 Loader B(3)이면,
			elif	Global_C_Auto_Action_Bit==L_Loader_B:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Loader_B_P1
			# Auto_Action_Bit가 Loader C(4)이면,
			elif	Global_C_Auto_Action_Bit==L_Loader_C:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Loader_C_P1
			# Auto_Action_Bit가 Cooker 1-1, 1-2, 2-1, 2-2, 3-1, Function A Common, Function B Common(5 ~ 9, 11, 13)이면,	
			elif	Global_C_Auto_Action_Bit==L_Cooker_1_1 or Global_C_Auto_Action_Bit==L_Cooker_1_2 or Global_C_Auto_Action_Bit==L_Cooker_2_1 or Global_C_Auto_Action_Bit==L_Cooker_2_2 or Global_C_Auto_Action_Bit==L_Cooker_3_1 or Global_C_Auto_Action_Bit==L_Cooker_Function_A_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Cooker_Common_P1
			# Auto_Action_Bit가 Cooker 3-2, Function A 3-2, Function B 3-2 (10, 12, 14)이면,
			elif	Global_C_Auto_Action_Bit==L_Cooker_3_2 or Global_C_Auto_Action_Bit==L_Cooker_Function_A_3_2 or Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
				# 변수에 Target 위치 값을 넣는다.
				L_Target_Pos=Global_P_Cooker_3_2_P1
			# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
			if	L_Target_Pos!=L_Interlock_Pos:
				# 이동 명령 상태 확인 변수 초기화
				Global_S_Auto_Check_Pos=-1
				# Loader A 투입 간섭 회피 위치 이동 명령 실행 후 결과 값을 리턴 한다
				Global_S_Auto_Check_Pos=movel(L_Target_Pos, vel=Global_C_Auto_Vel, acc=Global_C_Auto_Acc)
				# 이동 상태가 정상(0)일 경우에만 다음 Process 으로 넘긴다.
				if	Global_S_Auto_Check_Pos==0:
					# Grip 상태이면,
					if	Global_S_Grip == 1 and (Global_C_Auto_Action_Bit==L_Cooker_1_1 or Global_C_Auto_Action_Bit==L_Cooker_1_2 or Global_C_Auto_Action_Bit==L_Cooker_2_1 or Global_C_Auto_Action_Bit==L_Cooker_2_2 or Global_C_Auto_Action_Bit==L_Cooker_3_1 or Global_C_Auto_Action_Bit==L_Cooker_3_2):
						Global_S_Auto_Action_Step=80
					else:
						# 자동 동작 프로세스 스텝을 업데이트
						Global_S_Auto_Action_Step=90
				else:
					# 위치 이동 알람 활성화
					Global_S_Alarm_Pos_Move=1

		# Step_80 : 자동운전 옵션 (조리 완료 제품 기름 빼기)
		# 자동 동작 프로세스 스텝이 80이면,
		elif	Global_S_Auto_Action_Step==80:
			# 기름빼기 옵션이 1이면 실행한다.
			if	Global_C_Oil_Drain==1:
				# 반복 카운트 설정 ( Modbus로 받은 값을 넣어야 한다 )
				L_Oil_Drain_Cycle_Set_Count = Global_C_Oil_Drain_Count
				# 속도 설정
				L_Oil_Drain_Vel_H=1000
				L_Oil_Drain_Vel_L=1000
				# 가속도 설정
				L_Oil_Drain_Acc_H=1500
				L_Oil_Drain_Acc_L=1500
				# Auto_Action_Bit가 Cooker 1-1, 1-2, 2-1, 2-2, 3-1, Function A Common, Function B Common(5 ~ 9)이면,
				if	Global_C_Auto_Action_Bit==L_Cooker_1_1 or Global_C_Auto_Action_Bit==L_Cooker_1_2 or Global_C_Auto_Action_Bit==L_Cooker_2_1 or Global_C_Auto_Action_Bit==L_Cooker_2_2 or Global_C_Auto_Action_Bit==L_Cooker_3_1:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Wait
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Auto_Check_Pos=-1
						# Cooker1-1 ~ Cooker3-1 기름 빼기 대기 위치로 이동
						Global_S_Auto_Check_Pos=movel(L_Target_Pos, vel=Global_C_Auto_Vel, acc=Global_C_Auto_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Auto_Check_Pos==0:
							# 설정 된 카운트가 될때 까지 반복
							for i in range(0, L_Oil_Drain_Cycle_Set_Count): 
								# 모드 버스 143번지에서 읽은 Cobot 운전 속도 값 읽는다.
								Global_M_Read_Data[143]=get_modbus_slave(143)
								# Cobot 운전 속도 값이 0이 아닌 경우에만
								if	Global_M_Read_Data[143] != 0 and Global_M_Read_Data[143] != Global_C_Operation_Speed:
									# Cobot 속도 명령 변수 업데이트
									Global_C_Operation_Speed=Global_M_Read_Data[143]
									# Cobot 속도 변경 함수 실행
									change_operation_speed(Global_C_Operation_Speed)
									# Cobot 속도 상태 변수 업데이트
									Global_S_Operation_Speed=Global_C_Operation_Speed
									Global_M_Write_Data[159]=Global_S_Operation_Speed
									# 현재 속도 변경 값 쓰기
									set_modbus_slave(159, Global_M_Write_Data[159])
								# 급하강
								movel(Global_P_Oil_Drain_Common_1, vel=L_Oil_Drain_Vel_H, acc=L_Oil_Drain_Acc_H)
								# 상승
								movel(Global_P_Oil_Drain_Common_2, vel=L_Oil_Drain_Vel_L, acc=L_Oil_Drain_Acc_L)
							Global_S_Oil_Drain_Count = Global_C_Oil_Drain_Count
							# 자동 동작 프로세스 스텝을 다음 스텝으로 업데이트 한다.
							Global_S_Auto_Action_Step=90
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Auto_Action_Bit가 Cooker 3-2(10)이면,
				elif	Global_C_Auto_Action_Bit==L_Cooker_3_2:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Cooker_3_2_P1
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if		L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Auto_Check_Pos=-1
						# Cooker3-2 기름 빼기 대기 위치로 이동
						Global_S_Auto_Check_Pos=movel(L_Target_Pos, vel=Global_C_Auto_Vel, acc=Global_C_Auto_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Auto_Check_Pos==0:
						# 설정 된 카운트가 될때 까지 반복
							for i in range(0, L_Oil_Drain_Cycle_Set_Count):
								# 모드 버스 143번지에서 읽은 Cobot 운전 속도 값 읽는다.
								Global_M_Read_Data[143]=get_modbus_slave(143)
								# Cobot 운전 속도 값이 0이 아닌 경우에만
								if	Global_M_Read_Data[143] != 0 and Global_M_Read_Data[143] != Global_C_Operation_Speed:
									# Cobot 속도 명령 변수 업데이트
									Global_C_Operation_Speed=Global_M_Read_Data[143]
									# Cobot 속도 변경 함수 실행
									change_operation_speed(Global_C_Operation_Speed)
									# Cobot 속도 상태 변수 업데이트
									Global_S_Operation_Speed=Global_C_Operation_Speed
									Global_M_Write_Data[159]=Global_S_Operation_Speed
									# 현재 속도 변경 값 쓰기
									set_modbus_slave(159, Global_M_Write_Data[159])
								# 급하강
								movel(Global_P_Oil_Drain_3_2_1, vel=L_Oil_Drain_Vel_H, acc=L_Oil_Drain_Acc_H)
								# 상승
								movel(Global_P_Oil_Drain_3_2_2, vel=L_Oil_Drain_Vel_L, acc=L_Oil_Drain_Acc_L)
							# 오일 드레인 카운트 상태 값 써주기
							Global_S_Oil_Drain_Count = Global_C_Oil_Drain_Count
							# 자동 동작 프로세스 스텝을 업데이트
							Global_S_Auto_Action_Step=90
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Loader A,B,C, 조리중 흔들기, 조리 중 들고 있기(산소입히기)일 (2~4,11~14 )경우 
				elif	Global_C_Auto_Action_Bit==L_Loader_A or Global_C_Auto_Action_Bit==L_Loader_B or Global_C_Auto_Action_Bit==L_Loader_C or L_Cooker_Function_A_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_A_3_2 or Global_C_Auto_Action_Bit==L_Cooker_Function_B_Common or Global_C_Auto_Action_Bit==L_Cooker_Function_B_3_2:
					# 자동 동작 프로세스 스텝을 업데이트
					Global_S_Auto_Action_Step=90
			# 기름빼기 옵션이 0이면,
			elif	Global_C_Oil_Drain==0:
				# 자동 동작 프로세스 스텝을 업데이트
				Global_S_Auto_Action_Step=90

		# Step_90 : 대기 위치 이동
		# 자동 동작 프로세스 스텝이 90이면,
		elif	Global_S_Auto_Action_Step==90:
			# 이동 명령 상태 확인 변수 초기화
			Global_S_Auto_Check_Pos=-1
			# 대기 위치 이동 명령 실행 후 결과 값을 리턴 한다
			Global_S_Auto_Check_Pos= movel(Global_P_Wait, vel=Global_C_Auto_Vel, acc=Global_C_Auto_Acc)	
			# 이동 상태가 정상(0)일 경우에만
			if	Global_S_Auto_Check_Pos == 0:
				# 자동 동작 프로세스 스텝을 완료(100)으로 업데이트 한다.
				Global_S_Auto_Action_Step = 100	
			else:
				# 위치 이동 알람 활성화
				Global_S_Alarm_Pos_Move=1

		# 자동 동작 프로세스 스텝이 100이면,
		elif	Global_S_Auto_Action_Step==100:
			# 자동 동작 실행 완료 비트를 ON 한다 
			set_digital_output(16, ON)

# [ 자동 운전 실행 Program END ] ::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
# 운전 모드가 수동 운전 이면
elif	Global_C_IO_Bit == L_Manual_Mode:
	# 1-1. 자동 운전 변수 초기화
	Global_C_Auto_Action_Bit = 0
	Global_S_Auto_Action_Step = 0
	# 자동 동작 실행 완료 비트를 OFF 한다.
	set_digital_output(16, OFF)

	# 1-2. 수동 운전 프로세스 스텝 업데이트
	Global_M_Write_Data[153] = Global_S_Manual_Action_Step
	#모드 버스 153번지에 값을 적용 한다.
	set_modbus_slave(153, Global_M_Write_Data[153])

	# 1-3. 수동 운전 이벤트 확인
	Global_M_Read_Data[129]=get_modbus_slave(129)
	Global_C_Manual_Position_Bit = Global_M_Read_Data[129]
	# 수동 운전 명령 위치 확인
	if	Global_C_Manual_Position_Bit == L_Wait:
		Global_C_Manual_Position=Global_P_Wait
	elif	Global_C_Manual_Position_Bit == L_Loader_A:
		Global_C_Manual_Position=Global_P_Loader_A
	elif	Global_C_Manual_Position_Bit == L_Loader_B:
		Global_C_Manual_Position=Global_P_Loader_B
	elif	Global_C_Manual_Position_Bit == L_Loader_C:
		Global_C_Manual_Position=Global_P_Loader_C
	elif	Global_C_Manual_Position_Bit == L_Cooker_1_1:
		Global_C_Manual_Position=Global_P_Cooker_Common
	elif	Global_C_Manual_Position_Bit == L_Cooker_1_2:
		Global_C_Manual_Position=Global_P_Cooker_Common
	elif	Global_C_Manual_Position_Bit == L_Cooker_2_1:
		Global_C_Manual_Position=Global_P_Cooker_Common
	elif	Global_C_Manual_Position_Bit == L_Cooker_2_2:
		Global_C_Manual_Position=Global_P_Cooker_Common
	elif	Global_C_Manual_Position_Bit == L_Cooker_3_1:
		Global_C_Manual_Position=Global_P_Cooker_Common
	elif	Global_C_Manual_Position_Bit == L_Cooker_3_2:
		Global_C_Manual_Position=Global_P_Cooker_3_2
	elif	Global_C_Manual_Position_Bit == L_Tool_Measure:
		Global_C_Manual_Position=Global_P_Tool_Measure
	elif	Global_C_Manual_Position_Bit == L_Tool_Change:
		Global_C_Manual_Position=Global_P_Tool_Change
	#elif	Global_C_Manual_Position_Bit == L_Home:
	elif	Global_C_Manual_Position_Bit==L_Idle:
		Global_C_Manual_Position=Global_P_Cobot_Idle
	elif	Global_C_Manual_Position_Bit == L_Align_Master_Left:
		Global_C_Manual_Position=Global_P_Align_Master_Left
	elif	Global_C_Manual_Position_Bit == L_Align_Master_Right:
		Global_C_Manual_Position=Global_P_Align_Master_Right

	# 1-4. 수동 운전 실행
	# 수동 동작 이동 명령이 0이 아니면 실행
	if Global_C_Manual_Position_Bit != 0 and Global_M_Write_Data[145] == 0:
		# 이전이 대기 위치인 경우에만 실행
		if Global_S_Manual_Position_Bit == 0:
			# 프로세스 스텝이 0이면,
			if	Global_S_Manual_Action_Step == 0:
				# 프로세스 스텝을 시작 준비(10) 상태로 업데이트 한다.
				Global_S_Manual_Action_Step = 10
			# Step_10 위치 이동
			if	Global_S_Manual_Action_Step == 10:
				# Loader A(2) 이동 명령인 경우,
				if	Global_C_Manual_Position_Bit == L_Loader_A:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Loader_A_P1
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos=-1
						# Cooker3-2 기름 빼기 대기 위치로 이동
						Global_S_Manual_Check_Pos=movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos==0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 20
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Loader B(3) 이동 명령인 경우,
				elif	Global_C_Manual_Position_Bit == L_Loader_B:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Loader_B_P1
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos=-1
						# Cooker3-2 기름 빼기 대기 위치로 이동
						Global_S_Manual_Check_Pos=movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos==0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 20
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Loader C(4) 이동 명령인 경우,
				elif	Global_C_Manual_Position_Bit == L_Loader_C:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Loader_C_P1
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos=-1
						# Cooker3-2 기름 빼기 대기 위치로 이동
						Global_S_Manual_Check_Pos=movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos==0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 90
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Cooker 1-1, 1-2, 2-1, 2-2, 3-1(5~9) 이동 명령인 경우,
				elif	Global_C_Manual_Position_Bit == L_Cooker_1_1 or Global_C_Manual_Position_Bit == L_Cooker_1_2 or Global_C_Manual_Position_Bit == L_Cooker_2_1 or Global_C_Manual_Position_Bit == L_Cooker_2_2 or Global_C_Manual_Position_Bit == L_Cooker_3_1:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Cooker_Common_P1
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos=-1
						# Cooker3-2 기름 빼기 대기 위치로 이동
						Global_S_Manual_Check_Pos=movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos==0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 20
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Cooker 3-2(10) 이동 명령인 경우,
				elif	Global_C_Manual_Position_Bit == L_Cooker_3_2:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Cooker_3_2_P1
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos=-1
						# Cooker3-2 기름 빼기 대기 위치로 이동
						Global_S_Manual_Check_Pos=movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos==0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 20
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Tool Weight Measure(11) 이동 명령인 경우,
				elif	Global_C_Manual_Position_Bit == L_Tool_Measure:
					# 현재는 툴 무게 측정 위치와 대기 위치가 같으며, 이동 회피 위치는 필요 없다.
					# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
					Global_S_Manual_Action_Step = 90
				# Tool Change Measure(12) 이동 명령인 경우,
				elif	Global_C_Manual_Position_Bit == L_Tool_Change:
					# 현재는 툴 체인지 측정 위치와 대기 위치가 같으며, 이동 회피 위치는 필요 없다.
					# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
					Global_S_Manual_Action_Step = 90
				# Home(13) 이동 명령인 경우,
				elif	Global_C_Manual_Position_Bit == L_Home:
					# 현재 Task 위치 찾는 변수 초기화
					Global_Current_Task_Space=[0,0,0,0,0,0]
					Global_z=[0,0,0,0,0,0]
					L_Current_Solution_Space=0
					# 현재 Task 위치 값 읽어 오기
					Global_Current_Task_Space, L_Current_Solution_Space = list(get_current_posx(ref=DR_BASE))
					# 현재 위치를 변수에 넣기
					Global_z = Global_Current_Task_Space
					# 현재 Z축 위치 값과 대기 위치 Z값 보다 작으면, ( 실제로는 Y 값이 Z축 값인데 큰 값이 낮은 값이다 )
					if	Global_z[1] >= Global_P_Wait[1]:
						#현재 z축 위치 값을 대기 위치 z축 위치 값으로 넣는다. 
						Global_z[1] = Global_P_Wait[1]
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_z
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 90
					# 현재 Z축 위치 값과 대기 위치 Z값 보다 크면, ( 실제로는 Y 값이 Z축 값인데 작은 값이 높은 값이다 )
					elif	Global_z[1] <= Global_P_Wait[1]:
						# 변수에 Target 위치 값을 넣는다.
						L_Target_Pos=Global_P_Home_P1
						# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
						if	L_Target_Pos!=L_Interlock_Pos:
							# 이동 명령 상태 확인 변수 초기화
							Global_S_Manual_Check_Pos = -1
							# 이동 명령 실행 후 결과 값을 리턴
							Global_S_Manual_Check_Pos = movej(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
							# 이동 상태가 정상(0)일 경우에만
							if	Global_S_Manual_Check_Pos == 0:
								# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
								Global_S_Manual_Action_Step = 20
							else:
								# 위치 이동 알람 활성화
								Global_S_Alarm_Pos_Move=1
				# Idle(14) 이동 명령인 경우,
				elif	Global_C_Manual_Position_Bit == L_Idle:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Cobot_Idle_P1
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movej(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 20
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Align_Master_Left(15) 이동 명령인 경우,
				elif	Global_C_Manual_Position_Bit == L_Align_Master_Left:
					# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
					Global_S_Manual_Action_Step = 90
				# Align_Master_Right(16) 이동 명령인 경우,
				elif	Global_C_Manual_Position_Bit == L_Align_Master_Right:
					# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
					Global_S_Manual_Action_Step = 90

			# Step_20 간섭 회피 위치 2 이동
			elif	Global_S_Manual_Action_Step == 20:
				# Loader A(2) 이동 명령인 경우,
				if	Global_C_Manual_Position_Bit == L_Loader_A:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Loader_A_P2
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 90
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Loader B(3) 이동 명령인 경우,
				elif	Global_C_Manual_Position_Bit == L_Loader_B:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Loader_B_P2
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 90
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Cooker 1-1, 1-2, 2-1, 2-2, 3-1(5~9) 이동 명령인 경우,
				elif	Global_C_Manual_Position_Bit == L_Cooker_1_1 or Global_C_Manual_Position_Bit == L_Cooker_1_2 or Global_C_Manual_Position_Bit == L_Cooker_2_1 or Global_C_Manual_Position_Bit == L_Cooker_2_2 or Global_C_Manual_Position_Bit == L_Cooker_3_1:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Cooker_Common_P2
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 90
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Cooker 3-2(10) 이동 명령인 경우,
				elif	Global_C_Manual_Position_Bit == L_Cooker_3_2:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Cooker_3_2_P2
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 30
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Home(13) 이동 명령인 경우,
				elif	Global_C_Manual_Position_Bit == L_Home:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Home_P2
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movej(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 90
						else:
						# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Idle(14) 이동 명령인 경우,
				elif	Global_C_Manual_Position_Bit == L_Idle:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Cobot_Idle_P2
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movej(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 30
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1

			# Step_30 간섭 회피 위치 3 이동
			elif	Global_S_Manual_Action_Step == 30:
				# Cooker 3-2(10) 이동 명령인 경우,
				if	Global_C_Manual_Position_Bit == L_Cooker_3_2:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Cooker_3_2_P3
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 90
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Idle(14) 이동 명령인 경우,
				elif	Global_C_Manual_Position_Bit == L_Idle:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Cobot_Idle_P3
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movej(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 40
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1

			# Step_40 간섭 회피 위치 4 이동
			elif	Global_S_Manual_Action_Step == 40:
				# Idle(14) 이동 명령인 경우,
				if	Global_C_Manual_Position_Bit == L_Idle:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Cobot_Idle_P4
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movej(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 90
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1

			# Step_50
			elif	Global_S_Manual_Action_Step == 50:
				Global_S_Manual_Action_Step=60

			# Step_60
			elif	Global_S_Manual_Action_Step == 60:
				Global_S_Manual_Action_Step=70

			# Step_70
			elif	Global_S_Manual_Action_Step == 70:
				Global_S_Manual_Action_Step=80

			# Step_80
			elif	Global_S_Manual_Action_Step == 80:
				Global_S_Manual_Action_Step=90

			# Step_90 : 위치 이동
			elif	Global_S_Manual_Action_Step == 90:
				# Home(13) 이동 명령 이면
				if Global_C_Manual_Position_Bit == L_Home:
					# 이동 명령 상태 확인 변수 초기화
					Global_S_Manual_Check_Pos=-1
					# 이동 명령 실행 후 결과 값을 리턴
					Global_S_Manual_Check_Pos=move_home(DR_HOME_TARGET_USER)
					# 수동 동작 프로세스 스텝을 Complate(100)으로 업데이트 한다.
					Global_S_Manual_Action_Step=100
				# Cobot Idle 이동 명령 이면
				elif	Global_C_Manual_Position_Bit == L_Idle:	
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Cobot_Idle
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movej(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 100
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Home(13) 이동 명령 과 Cobot Idle 이동 명령이 아니면
				elif	Global_C_Manual_Position_Bit != L_Home and Global_C_Manual_Position_Bit != L_Idle:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_C_Manual_Position
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 100
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1

			# Step_100
			elif	Global_S_Manual_Action_Step == 100:
				# 수동 동작 완료 위치를 명령 받은 위치로 동일하게 입력 한다. ( 수동 운전에서 배출 프로세스를 태우기 위해 이전 위치 값을 저장 )
				Global_S_Manual_Position_Bit=Global_C_Manual_Position_Bit
				Global_M_Write_Data[145]=Global_S_Manual_Position_Bit
				#모드 버스 145번지에 값을 적용 한다.
				set_modbus_slave(145, Global_M_Write_Data[145])

		# 이전이 대기 위치가 0이 아닌 경우에만 실행
		elif Global_S_Manual_Position_Bit != 0 and Global_M_Write_Data[145] == 0:
			# 프로세스 스텝이 0이면,
			if	Global_S_Manual_Action_Step == 0:
				# 프로세스 스텝을 시작 준비(10) 상태로 업데이트 한다.
				Global_S_Manual_Action_Step = 10
			# Step_10 위치 이동
			elif	Global_S_Manual_Action_Step == 10:
				# Loader A(2) 이동 명령인 경우,
				if	Global_S_Manual_Position_Bit == L_Loader_A:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Loader_A_P2
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 20
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Loader B(3) 이동 명령인 경우,
				elif	Global_S_Manual_Position_Bit == L_Loader_B:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Loader_B_P2
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 20
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Loader C(4) 이동 명령인 경우,
				elif	Global_S_Manual_Position_Bit == L_Loader_C:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Loader_C_P1
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 90
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Cooker 1-1, 1-2, 2-1, 2-2, 3-1(5~9) 이동 명령인 경우,
				elif	Global_S_Manual_Position_Bit == L_Cooker_1_1 or Global_S_Manual_Position_Bit == L_Cooker_1_2 or Global_S_Manual_Position_Bit == L_Cooker_2_1 or Global_S_Manual_Position_Bit == L_Cooker_2_2 or Global_S_Manual_Position_Bit == L_Cooker_3_1:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Cooker_Common_P2
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 20
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Cooker 3-2(10) 이동 명령인 경우,
				elif	Global_S_Manual_Position_Bit == L_Cooker_3_2:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Cooker_3_2_P3
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 20
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Tool Weight Measure(11) 이동 명령인 경우,
				elif	Global_S_Manual_Position_Bit == L_Tool_Measure :
					# 현재는 툴 무게 측정 위치와 대기 위치가 같으며, 이동 회피 위치는 필요 없다.
					# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
					Global_S_Manual_Action_Step = 90
				# Tool Change Measure(12) 이동 명령인 경우,
				elif	Global_S_Manual_Position_Bit == L_Tool_Change :
					# 현재는 툴 체인지 측정 위치와 대기 위치가 같으며, 이동 회피 위치는 필요 없다.
					# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
					Global_S_Manual_Action_Step = 90
				# Idle(14) 이동 명령인 경우,
				elif	Global_S_Manual_Position_Bit == L_Idle :
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Cobot_Idle_P4
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movej(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 20
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# _Align_Master_Left (15) 이동 명령인 경우,
				elif	Global_S_Manual_Position_Bit == L_Align_Master_Left:
					# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
					Global_S_Manual_Action_Step = 90
				# Align_Master_Right(16) 이동 명령인 경우,
				elif	Global_S_Manual_Position_Bit == L_Align_Master_Right:
					# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
					Global_S_Manual_Action_Step = 90
			# Step_20 간섭 회피 위치 2 이동
			elif	Global_S_Manual_Action_Step == 20:
				# Loader A(2) 이동 명령인 경우,
				if	Global_S_Manual_Position_Bit == L_Loader_A:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Loader_A_P1
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 90
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Loader B(3) 이동 명령인 경우,
				elif	Global_S_Manual_Position_Bit == L_Loader_B:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Loader_B_P1
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 90
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Cooker 1-1, 1-2, 2-1, 2-2, 3-1(5~9) 동 명령인 경우,
				elif	Global_S_Manual_Position_Bit == L_Cooker_1_1 or Global_S_Manual_Position_Bit == L_Cooker_1_2 or Global_S_Manual_Position_Bit == L_Cooker_2_1 or Global_S_Manual_Position_Bit == L_Cooker_2_2 or Global_S_Manual_Position_Bit == L_Cooker_3_1:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Cooker_Common_P1
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 90
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Cooker 3-2(10) 이동 명령인 경우,
				elif	Global_S_Manual_Position_Bit == L_Cooker_3_2:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Cooker_3_2_P2
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 30
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1

				# Idle(14) 이동 명령인 경우,
				elif	Global_S_Manual_Position_Bit == L_Idle:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Cobot_Idle_P3
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movej(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 30
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1

			# Step_30 간섭 회피 위치 3 이동
			elif	Global_S_Manual_Action_Step == 30:
				# Cooker 3-2(10) 이동 명령인 경우,
				if	Global_S_Manual_Position_Bit == L_Cooker_3_2:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Cooker_3_2_P1
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 90
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
				# Idle(14) 이동 명령인 경우,
				elif	Global_S_Manual_Position_Bit == L_Idle:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Cobot_Idle_P2
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movej(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 40
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1

			# Step_40 간섭 회피 위치 4 이동
			elif	Global_S_Manual_Action_Step == 40:
				# Idle(14) 이동 명령인 경우,
				if	Global_S_Manual_Position_Bit == L_Idle:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_P_Cobot_Idle_P1
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movej(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 90
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1

			# Step_50
			elif	Global_S_Manual_Action_Step == 50:
				Global_S_Manual_Action_Step=60	

			# Step_60
			elif	Global_S_Manual_Action_Step == 60:
				Global_S_Manual_Action_Step=70

			# Step_70
			elif	Global_S_Manual_Action_Step == 70:
				Global_S_Manual_Action_Step=80

			# Step_80
			elif	Global_S_Manual_Action_Step == 80:
				Global_S_Manual_Action_Step=90

			# Step_90 목표 위치 이동
			elif	Global_S_Manual_Action_Step == 90:
				# Home(13) 이동 명령이 아니면
				if	Global_S_Manual_Position_Bit != L_Home:
					# 변수에 Target 위치 값을 넣는다.
					L_Target_Pos=Global_C_Manual_Position
					# 타겟 위치 값이 있을 경우 에만 실행 되도록 처리
					if	L_Target_Pos!=L_Interlock_Pos:
						# 이동 명령 상태 확인 변수 초기화
						Global_S_Manual_Check_Pos = -1
						# 이동 명령 실행 후 결과 값을 리턴
						Global_S_Manual_Check_Pos = movel(L_Target_Pos, vel=Global_C_Manual_Vel, acc=Global_C_Manual_Acc)
						# 이동 상태가 정상(0)일 경우에만
						if	Global_S_Manual_Check_Pos == 0:
							# 수동 동작 프로세스 스텝을 다음 Process 으로 넘긴다.
							Global_S_Manual_Action_Step = 100
						else:
							# 위치 이동 알람 활성화
							Global_S_Alarm_Pos_Move=1
			# Step_100 목표 위치 이동
			elif	Global_S_Manual_Action_Step == 100:
				# 이전 실행 된 수동 동작 위치를 초기화(0) 한다.
				#Global_S_Manual_Position_Bit=0
				# 수동 동작 완료 위치를 명령 받은 위치로 동일하게 입력 한다. ( 수동 운전에서 배출 프로세스를 태우기 위해 이전 위치 값을 저장 )
				Global_S_Manual_Position_Bit=Global_C_Manual_Position_Bit
				Global_M_Write_Data[145]=Global_S_Manual_Position_Bit
				#모드 버스 145번지에 값을 적용 한다.
				set_modbus_slave(145, Global_M_Write_Data[145])

	# 수동 동작 이동 명령이 0이면 실행
	elif	Global_C_Manual_Position_Bit == 0:
		# 프로세스 스텝을 0으로 업데이트 한다.
		Global_S_Manual_Action_Step = 0
		# 수동 운전 프로세스 스텝 업데이트
		Global_M_Write_Data[145]=0
		#모드 버스 145번지에 값을 적용 한다.
		set_modbus_slave(145, Global_M_Write_Data[145])
		if	Global_S_Manual_Position_Bit == L_Wait or Global_S_Manual_Position_Bit == L_Home:
			Global_S_Manual_Position_Bit=0			
# [ 수동 운전 실행 Program END ] ::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::