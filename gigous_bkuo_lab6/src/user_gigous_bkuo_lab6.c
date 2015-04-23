#include <std.h>
#include <log.h>
#include <clk.h>
#include <gbl.h>
#include <bcache.h>

#include <mem.h> // MEM_alloc calls
#include <que.h> // QUE functions
#include <sem.h> // Semaphore functions
#include <sys.h> 
#include <tsk.h> // TASK functions
#include <math.h> 
#include <stdio.h> 
#include <stdlib.h>
#include <string.h>
#include <c6x.h> // register defines


#include "projectinclude.h"
#include "c67fastMath.h" // sinsp,cossp, tansp
#include "evmomapl138.h"
#include "evmomapl138_i2c.h"
#include "evmomapl138_timer.h"
#include "evmomapl138_led.h"
#include "evmomapl138_dip.h"
#include "evmomapl138_gpio.h"
#include "evmomapl138_vpif.h"
#include "evmomapl138_spi.h"
#include "COECSL_edma3.h"
#include "COECSL_mcbsp.h"
#include "COECSL_registers.h"

#include "mcbsp_com.h"
#include "ColorVision.h"
#include "ColorLCD.h"
#include "sharedmem.h"
#include "LCDprintf.h"
#include "ladar.h"
#include "xy.h"
#include "MatrixMath.h"

#define FEETINONEMETER 3.28083989501312

#define REF_RIGHT_WALL 2
#define FRONT_ERROR_THRESHOLD 3
#define KP_RIGHT_WALL 4
#define KP_FRONT_WALL 5
#define FRONT_TURN_VELOCITY 6
#define TURN_COMMAND_SATURATION 7

// #define GYRO_K ()
// #define GYRO_K_AMP

extern EDMA3_CCRL_Regs *EDMA3_0_Regs;

volatile uint32_t index;

// test variables
extern float enc1;  // Left motor encoder
extern float enc2;  // Right motor encoder
extern float enc3;
extern float enc4;
extern float adcA0;  // ADC A0 - Gyro_X -400deg/s to 400deg/s  Pitch
extern float adcB0;  // ADC B0 - External ADC Ch4 (no protection circuit)
extern float adcA1;  // ADC A1 - Gyro_4X -100deg/s to 100deg/s  Pitch
extern float adcB1;  // ADC B1 - External ADC Ch1
extern float adcA2;  // ADC A2 -	Gyro_4Z -100deg/s to 100deg/s  Yaw
extern float adcB2;  // ADC B2 - External ADC Ch2
extern float adcA3;  // ADC A3 - Gyro_Z -400deg/s to 400 deg/s  Yaw
extern float adcB3;  // ADC B3 - External ADC Ch3
extern float adcA4;  // ADC A4 - Analog IR1
extern float adcB4;  // ADC B4 - USONIC1
extern float adcA5;  // ADC A5 -	Analog IR2
extern float adcB5;  // ADC B5 - USONIC2
extern float adcA6;  // ADC A6 - Analog IR3
extern float adcA7;  // ADC A7 - Analog IR4
extern float compass;
extern float switchstate;

float vref = 0;
float turn = 0;
float ref_right_wall = 500;
float front_error_threshold = 2500;
float Kp_right_wall = 0.002;
float Kp_front_wall = 0.001;
float front_turn_velocity = 0.4;
float turn_command_saturation = 1.0;

int newnavdata = 0;
float newvref = 0;
float newturn = 0;
float new_ref_right_wall = 500;
float new_front_error_threshold = 2500;
float new_Kp_right_wall = 0.002;
float new_Kp_front_wall = 0.001;
float new_front_turn_velocity = 0.4;
float new_turn_command_saturation = 1.0;

int tskcount = 0;
char fromLinuxstring[LINUX_COMSIZE + 2];
char toLinuxstring[LINUX_COMSIZE + 2];

float LVvalue1 = 0;
float LVvalue2 = 0;
int new_LV_data = 0;

float front_wall_error;
float right_wall_error;

extern sharedmemstruct *ptrshrdmem;

extern pose ROBOTps;
extern pose LADARps;

extern float newLADARdistance[LADAR_MAX_DATA_SIZE];  //in mm
extern float newLADARangle[LADAR_MAX_DATA_SIZE];		// in degrees
float LADARdistance[LADAR_MAX_DATA_SIZE];
float LADARangle[LADAR_MAX_DATA_SIZE];
extern float newLADARdataX[LADAR_MAX_DATA_SIZE];
extern float newLADARdataY[LADAR_MAX_DATA_SIZE];
float LADARdataX[LADAR_MAX_DATA_SIZE];
float LADARdataY[LADAR_MAX_DATA_SIZE];
extern int newLADARdata;

// Optitrack Variables
int trackableIDerror = 0;
int firstdata = 1;
volatile int new_optitrack = 0;
volatile float previous_frame = -1;
int frame_error = 0;
volatile float Optitrackdata[OPTITRACKDATASIZE];
pose OPTITRACKps;
float temp_theta = 0.0;
float tempOPTITRACK_theta = 0.0;
volatile int temp_trackableID = -1;
int trackableID = -1;
int errorcheck = 1;




unsigned int dummy = 0;

float gyro_zero = 0;
float gyro_zero_amp = 0;

float gyro_val = 0;
float gyro_val_amp = 0;

float prev_gyro_val = 0;
float prev_gyro_val_amp = 0;

float gyro_gain400 = 400.0*(PI/180.0);
float gyro_gain100 = 100.0*(PI/180.0);
float gyro_gain = 1.0;

float angle_integral = 0;
float angle_integral_amp = 0;

float p_current1 = 0;
float p_old1 = 0;
float p_current2 = 0;
float p_old2 = 0;

float v1 = 0;
float v2 = 0;

float pos_x = 0.0;
float pos_y = 0.0;

pose UpdateOptitrackStates(pose localROBOTps, int * flag);

void swapNumbers(int i, int j, float *array) {

        float temp;
        temp = array[i];
        array[i] = array[j];
        array[j] = temp;
}

void bubbleSort(float *array) {
        int n = 5;
        int k;
        int m;
        int i;
        for (m = n; m >= 0; m--) {
            for (i = 0; i < m - 1; i++) {
                k = i + 1;
                if (array[i] > array[k]) {
                    swapNumbers(i, k, array);
                }
            }
            //printNumbers(array);
        }
}

void ComWithLinux(void) {

	int i = 0;
	TSK_sleep(100);

	while(1) {

		BCACHE_inv((void *)ptrshrdmem,sizeof(sharedmemstruct),EDMA3_CACHE_WAIT);
		
		if (GET_DATA_FROM_LINUX) {

			if (newnavdata == 0) {

				newvref = ptrshrdmem->Floats_to_DSP[0];
				newturn = ptrshrdmem->Floats_to_DSP[1];
				new_ref_right_wall = ptrshrdmem->Floats_to_DSP[REF_RIGHT_WALL];
				new_front_error_threshold = ptrshrdmem->Floats_to_DSP[FRONT_ERROR_THRESHOLD];
				new_Kp_right_wall = ptrshrdmem->Floats_to_DSP[KP_RIGHT_WALL];
				new_Kp_front_wall = ptrshrdmem->Floats_to_DSP[KP_FRONT_WALL];
				new_front_turn_velocity = ptrshrdmem->Floats_to_DSP[FRONT_TURN_VELOCITY];
				new_turn_command_saturation = ptrshrdmem->Floats_to_DSP[TURN_COMMAND_SATURATION];

				newnavdata = 1;
			}

			CLR_DATA_FROM_LINUX;

		}

		if (GET_LVDATA_FROM_LINUX) {

			if (ptrshrdmem->DSPRec_size > 256) ptrshrdmem->DSPRec_size = 256;
				for (i=0;i<ptrshrdmem->DSPRec_size;i++) {
					fromLinuxstring[i] = ptrshrdmem->DSPRec_buf[i];
				}
				fromLinuxstring[i] = '\0';

				if (new_LV_data == 0) {
					sscanf(fromLinuxstring,"%f%f",&LVvalue1,&LVvalue2);
					new_LV_data = 1;
				}

			CLR_LVDATA_FROM_LINUX;

		}

		if ((tskcount%6)==0) {
			if (GET_LVDATA_TO_LINUX) {

				// Default
				ptrshrdmem->DSPSend_size = sprintf(toLinuxstring,"%.1f %.1f %.1f %.1f", pos_x, pos_y, angle_integral, angle_integral_amp);
				// you would do something like this
				//ptrshrdmem->DSPSend_size = sprintf(toLinuxstring,"%.1f %.1f %.1f %.1f",var1,var2,var3,var4);

				for (i=0;i<ptrshrdmem->DSPSend_size;i++) {
					ptrshrdmem->DSPSend_buf[i] = toLinuxstring[i];
				}

				// Flush or write back source
				BCACHE_wb((void *)ptrshrdmem,sizeof(sharedmemstruct),EDMA3_CACHE_WAIT);

				CLR_LVDATA_TO_LINUX;

			}
		}
		
		if (GET_DATAFORFILE_TO_LINUX) {

			// This is an example write to scratch
			// The Linux program SaveScratchToFile can be used to write the
			// ptrshrdmem->scratch[0-499] memory to a .txt file.
//			for (i=100;i<300;i++) {
//				ptrshrdmem->scratch[i] = (float)i;
//			}

			// Flush or write back source
			BCACHE_wb((void *)ptrshrdmem,sizeof(sharedmemstruct),EDMA3_CACHE_WAIT);

			CLR_DATAFORFILE_TO_LINUX;

		}

		tskcount++;
		TSK_sleep(40);
	}


}


/*
 *  ======== main ========
 */
Void main()
{

	int i = 0;

	// unlock the system config registers.
	SYSCONFIG->KICKR[0] = KICK0R_UNLOCK;
	SYSCONFIG->KICKR[1] = KICK1R_UNLOCK;

	SYSCONFIG1->PUPD_SEL |= 0x10000000;  // change pin group 28 to pullup for GP7[12/13] (LCD switches)

	// Initially set McBSP1 pins as GPIO ins
	CLRBIT(SYSCONFIG->PINMUX[1], 0xFFFFFFFF);
	SETBIT(SYSCONFIG->PINMUX[1], 0x88888880);  // This is enabling the McBSP1 pins

	CLRBIT(SYSCONFIG->PINMUX[16], 0xFFFF0000);
	SETBIT(SYSCONFIG->PINMUX[16], 0x88880000);  // setup GP7.8 through GP7.13 
	CLRBIT(SYSCONFIG->PINMUX[17], 0x000000FF);
	SETBIT(SYSCONFIG->PINMUX[17], 0x00000088);  // setup GP7.8 through GP7.13


	//Rick added for LCD DMA flagging test
	GPIO_setDir(GPIO_BANK0, GPIO_PIN8, GPIO_OUTPUT);
	GPIO_setOutput(GPIO_BANK0, GPIO_PIN8, OUTPUT_HIGH);

	GPIO_setDir(GPIO_BANK0, GPIO_PIN0, GPIO_INPUT);
	GPIO_setDir(GPIO_BANK0, GPIO_PIN1, GPIO_INPUT);
	GPIO_setDir(GPIO_BANK0, GPIO_PIN2, GPIO_INPUT);
	GPIO_setDir(GPIO_BANK0, GPIO_PIN3, GPIO_INPUT);
	GPIO_setDir(GPIO_BANK0, GPIO_PIN4, GPIO_INPUT);
	GPIO_setDir(GPIO_BANK0, GPIO_PIN5, GPIO_INPUT);  
	GPIO_setDir(GPIO_BANK0, GPIO_PIN6, GPIO_INPUT);

	GPIO_setDir(GPIO_BANK7, GPIO_PIN8, GPIO_OUTPUT);
	GPIO_setDir(GPIO_BANK7, GPIO_PIN9, GPIO_OUTPUT);
	GPIO_setDir(GPIO_BANK7, GPIO_PIN10, GPIO_OUTPUT);
	GPIO_setDir(GPIO_BANK7, GPIO_PIN11, GPIO_OUTPUT);
	GPIO_setDir(GPIO_BANK7, GPIO_PIN12, GPIO_INPUT);
	GPIO_setDir(GPIO_BANK7, GPIO_PIN13, GPIO_INPUT); 

	GPIO_setOutput(GPIO_BANK7, GPIO_PIN8, OUTPUT_HIGH);  
	GPIO_setOutput(GPIO_BANK7, GPIO_PIN9, OUTPUT_HIGH);
	GPIO_setOutput(GPIO_BANK7, GPIO_PIN10, OUTPUT_HIGH);
	GPIO_setOutput(GPIO_BANK7, GPIO_PIN11, OUTPUT_HIGH);  

	CLRBIT(SYSCONFIG->PINMUX[13], 0xFFFFFFFF);
	SETBIT(SYSCONFIG->PINMUX[13], 0x88888811); //Set GPIO 6.8-13 to GPIOs and IMPORTANT Sets GP6[15] to /RESETOUT used by PHY, GP6[14] CLKOUT appears unconnected

	#warn GP6.15 is also connected to CAMERA RESET This is a Bug in my board design Need to change Camera Reset to different IO.

	GPIO_setDir(GPIO_BANK6, GPIO_PIN8, GPIO_OUTPUT);
	GPIO_setDir(GPIO_BANK6, GPIO_PIN9, GPIO_OUTPUT);
	GPIO_setDir(GPIO_BANK6, GPIO_PIN10, GPIO_OUTPUT);
	GPIO_setDir(GPIO_BANK6, GPIO_PIN11, GPIO_OUTPUT);
	GPIO_setDir(GPIO_BANK6, GPIO_PIN12, GPIO_OUTPUT);
	GPIO_setDir(GPIO_BANK6, GPIO_PIN13, GPIO_INPUT);   


   // on power up wait until Linux has initialized Timer1
	while ((T1_TGCR & 0x7) != 0x7) {
	  for (index=0;index<50000;index++) {}  // small delay before checking again

	}

	USTIMER_init();
	
	// Turn on McBSP1
	EVMOMAPL138_lpscTransition(PSC1, DOMAIN0, LPSC_MCBSP1, PSC_ENABLE);

    // If Linux has already booted It sets a flag so no need to delay
    if ( GET_ISLINUX_BOOTED == 0) {
    	USTIMER_delay(4*DELAY_1_SEC);  // delay allowing Linux to partially boot before continuing with DSP code
    }
	   
	// init the us timer and i2c for all to use.
	I2C_init(I2C0, I2C_CLK_100K);
	init_ColorVision();	
	init_LCD_mem(); // added rick

	EVTCLR0 = 0xFFFFFFFF;
	EVTCLR1 = 0xFFFFFFFF;
	EVTCLR2 = 0xFFFFFFFF;
	EVTCLR3 = 0xFFFFFFFF;	

	init_DMA();
	init_McBSP();

	init_LADAR();

	CLRBIT(SYSCONFIG->PINMUX[1], 0xFFFFFFFF);
	SETBIT(SYSCONFIG->PINMUX[1], 0x22222220);  // This is enabling the McBSP1 pins

	CLRBIT(SYSCONFIG->PINMUX[5], 0x00FF0FFF);
	SETBIT(SYSCONFIG->PINMUX[5], 0x00110111);  // This is enabling SPI pins

	CLRBIT(SYSCONFIG->PINMUX[16], 0xFFFF0000);
	SETBIT(SYSCONFIG->PINMUX[16], 0x88880000);  // setup GP7.8 through GP7.13 
	CLRBIT(SYSCONFIG->PINMUX[17], 0x000000FF);
	SETBIT(SYSCONFIG->PINMUX[17], 0x00000088);  // setup GP7.8 through GP7.13

	init_LCD();
    
	LADARps.x = 3.5/12; // 3.5/12 for front mounting
	LADARps.y = 0;
	LADARps.theta = 1;  // not inverted

	OPTITRACKps.x = 0;
	OPTITRACKps.y = 0;
	OPTITRACKps.theta = 0;

	for(i = 0;i<LADAR_MAX_DATA_SIZE;i++)
	{ LADARdistance[i] = LADAR_MAX_READING; } //initialize all readings to max value.

	// ROBOTps will be updated by Optitrack during gyro calibration
	// TODO: specify the starting position of the robot
	ROBOTps.x = 0;			//the estimate in array form (useful for matrix operations)
	ROBOTps.y = 0;
	ROBOTps.theta = 0;  // was -PI: need to flip OT ground plane to fix this

	// flag pins
	GPIO_setDir(IMAGE_TO_LINUX_BANK, IMAGE_TO_LINUX_FLAG, GPIO_OUTPUT);
	GPIO_setDir(OPTITRACKDATA_FROM_LINUX_BANK, OPTITRACKDATA_FROM_LINUX_FLAG, GPIO_OUTPUT);
	GPIO_setDir(DATA_TO_LINUX_BANK, DATA_TO_LINUX_FLAG, GPIO_OUTPUT);
	GPIO_setDir(DATA_FROM_LINUX_BANK, DATA_FROM_LINUX_FLAG, GPIO_OUTPUT);
	GPIO_setDir(DATAFORFILE_TO_LINUX_BANK, DATAFORFILE_TO_LINUX_FLAG, GPIO_OUTPUT);
	GPIO_setDir(LVDATA_FROM_LINUX_BANK, LVDATA_FROM_LINUX_FLAG, GPIO_OUTPUT);
	GPIO_setDir(LVDATA_TO_LINUX_BANK, LVDATA_TO_LINUX_FLAG, GPIO_OUTPUT);


	CLR_OPTITRACKDATA_FROM_LINUX;  // Clear = tell linux DSP is ready for new Opitrack data
	CLR_DATA_FROM_LINUX;  // Clear = tell linux that DSP is ready for new data
	CLR_DATAFORFILE_TO_LINUX;  // Clear = linux not requesting data
	SET_DATA_TO_LINUX;  // Set = put float array data into shared memory for linux
	SET_IMAGE_TO_LINUX;  // Set = put image into shared memory for linux
	CLR_LVDATA_FROM_LINUX;  // Clear = tell linux that DSP is ready for new LV data
	SET_LVDATA_TO_LINUX;  // Set = put LV char data into shared memory for linux

    // clear all possible EDMA 
	EDMA3_0_Regs->SHADOW[1].ICR = 0xFFFFFFFF;
	
    // Add your init code here
}
	

long timecount= 0;
int whichled = 0;
// This SWI is Posted after each set of new data from the F28335
//Posted every millisecond
void RobotControl(void) {

	int newOPTITRACKpose = 0;
	int i = 0;

	if (0==(timecount%1000)) {
		switch(whichled) {
		case 0:
			SETREDLED;
			CLRBLUELED;
			CLRGREENLED;
			whichled = 1;
			break;
		case 1:
			CLRREDLED;
			SETBLUELED;
			CLRGREENLED;
			whichled = 2;
			break;
		case 2:
			CLRREDLED;
			CLRBLUELED;
			SETGREENLED;
			whichled = 0;
			break;
		default:
			whichled = 0;
			break;
		}
	}
	
	if (GET_OPTITRACKDATA_FROM_LINUX) {
		if (new_optitrack == 0) {
			for (i=0;i<OPTITRACKDATASIZE;i++) {
				Optitrackdata[i] = ptrshrdmem->Optitrackdata[i];
			}
			new_optitrack = 1;
		}
		CLR_OPTITRACKDATA_FROM_LINUX;
	}

	if (new_optitrack == 1) {
		OPTITRACKps = UpdateOptitrackStates(ROBOTps, &newOPTITRACKpose);
		new_optitrack = 0;
	}

	if (newLADARdata == 1) {
		newLADARdata = 0;
		for (i=0;i<228;i++) {
			LADARdistance[i] = newLADARdistance[i];
			LADARangle[i] = newLADARangle[i];
			LADARdataX[i] = newLADARdataX[i];
			LADARdataY[i] = newLADARdataY[i];
		}
	}

	if (new_LV_data == 1)
	{
		gyro_gain = LVvalue1;
		vref = LVvalue2;
		new_LV_data = 0;
	}

	timecount++;
	/*gyro code!*/	
	if(timecount < 3000)//get zero value
	{
		// gyro_zero += adcA3*(1.0/2999.0);
		// gyro_zero_amp += adcA2*(1.0/2999.0);
		gyro_zero += adcA3;
		gyro_zero_amp += adcA2;
		if ((timecount % 100) == 1){
			LCDPrintfLine(1,"Getting gyro zero");
			LCDPrintfLine(2,"%d%% complete", timecount*100/3000);
		}
		SetRobotOutputs(0,0,0,0,0,0,0,0,0,0);
		return;
	}
	else if (timecount == 3000)
	{
		gyro_zero /= 2999.0;
		gyro_zero_amp /= 2999.0;
		pos_x = 0.0;
		pos_y = 0.0;
		p_old1=p_current1;
		p_old2=p_current2;
	}

	gyro_val = gyro_gain*(adcA3 - gyro_zero)*(3.0/4095)*gyro_gain400;
	gyro_val_amp = gyro_gain*(adcA2 - gyro_zero_amp)*(3.0/4095)*gyro_gain100;

	//finding integral

	angle_integral += (prev_gyro_val+gyro_val)/2.0*.001;
	angle_integral_amp += (prev_gyro_val_amp+gyro_val_amp)/2.0*.001;

	prev_gyro_val = gyro_val;
	prev_gyro_val_amp = gyro_val_amp;

	//velocity and position calculations
	p_current1 = enc1/192.0;
	p_current2 = enc2/192.0;
	if(timecount == 3000) {
		p_old1=p_current1;
		p_old2=p_current2;
	}

	v1 = (p_current1-p_old1)/0.001;
	v2 = (p_current2-p_old2)/0.001;

	pos_x = pos_x + ((v1+v2)/2.0*0.001)*cosf(angle_integral_amp);
	pos_y = pos_y + ((v1+v2)/2.0*0.001)*sinf(angle_integral_amp);

	p_old1 = p_current1;
	p_old2 = p_current2;


	// Get rid of these two lines when implementing wall following
	// vref = 0.0;
	// turn = 0.0;

	//for excersize 1
	//vref = 1.0;
	//turn = enc3/100;

	if (newnavdata)
	{
		vref = newvref;
		turn = newturn;
		ref_right_wall = new_ref_right_wall;
		front_error_threshold = new_front_error_threshold;
		Kp_right_wall = new_Kp_right_wall;
		Kp_front_wall = new_Kp_front_wall;
		front_turn_velocity = new_front_turn_velocity;
		turn_command_saturation = new_turn_command_saturation;

		newnavdata = 0;
	}
	
	// if ((LADARdistance[111] < 500)||
	// 	(LADARdistance[112] < 500)||
	// 	(LADARdistance[113] < 500)||
	// 	(LADARdistance[114] < 500)||
	// 	(LADARdistance[115] < 500))
	// {
	// 	vref = 0;
	// }

	// Add declarations for tunable global variables to include:
	// ref_right_wall - desired distance from right wall, approx 0.8 tiles. You will have
	// to figure out what distance is read by the LADAR [54] reading when the robot is
	// placed approximately 0.8 tiles from the wall.
	// front_error_threshold - maximum distance from a front wall before you stop
	// wall-following and switch to a front wall-avoidance mode, start with a
	// front_wall_error that occurs when the robot it 1 tile away for the front wall.
	// Kp_right_wall - proportional gain for controlling distance of robot to wall,
	// start with 0.002
	// Kp_front_wall - proportional gain for turning robot when front wall error is high,
	// start with 0.001
	// front_turn_velocity - velocity when the robot starts to turn to avoid
	// a front wall, use 0.4 to start
	// forward_velocity - velocity when robot is right wall following, use 1.0 to start
	// turn_command_saturation - maximum turn command to prevent robot from spinning quickly if
	// error jumps too high, start with 1.0
	// These are all ‘knobs’ to tune in lab!
	// declare other globals that you will need

	// inside RobotControl()

	float front_ladar[5];
	float right_ladar[5];

	front_ladar[0] = LADARdistance[111];
	front_ladar[1] = LADARdistance[112];
	front_ladar[2] = LADARdistance[113];
	front_ladar[3] = LADARdistance[114];
	front_ladar[4] = LADARdistance[115];

	right_ladar[0] = LADARdistance[52];
	right_ladar[1] = LADARdistance[53];
	right_ladar[2] = LADARdistance[54];
	right_ladar[3] = LADARdistance[55];
	right_ladar[4] = LADARdistance[56];

	bubbleSort(front_ladar);
	bubbleSort(right_ladar);
	
	// calculate front wall error (3000.0 - front wall distance)
	
	front_wall_error = 3000.0 - front_ladar[0];
	right_wall_error = ref_right_wall - right_ladar[0];

	// calculate error between ref_right_wall and right wall measurement
	
	if (front_wall_error > front_error_threshold){
		// Change turn command according to proportional feedback control on front error
		// use Kp_front_wall here…

		turn = -Kp_front_wall*front_wall_error;
		vref = front_turn_velocity;
		dummy += 1;
	}
	else {
		// Change turn command according to proportional feedback control on right error
		// use Kp_right_wall here
		//	vref = forward_velocity

		// Add code here to saturate the turn command so that it is not larger
		// than turn_command_saturation or less than -turn_command_saturation
		turn = -Kp_right_wall*right_wall_error;
		if (turn > turn_command_saturation)
		{
			turn = turn_command_saturation;
		}
		else if (turn < -turn_command_saturation)
		{
			turn = -turn_command_saturation;
		}
		vref = newvref;
	}

	SetRobotOutputs(vref+LVvalue2,turn,0,0,0,0,0,0,0,0);

	adcA4 = 4096 - adcA4;
	adcA5 = 4096 - adcA5;
	adcA6 = 4096 - adcA6;
	adcA7 = 4096 - adcA7;

	if ((timecount % 100) == 1)
	{
		switch ((int) switchstate)
		{
			case 0:
				LCDPrintfLine(1,"LADAR %.1f %.1f", LADARdistance[1], LADARdistance[225]);
				LCDPrintfLine(2,"%.1f %.1f %.1f", LADARdistance[172], LADARdistance[54], LADARdistance[113]);
				break;
			case 1: 
				LCDPrintfLine(1,"IR READINGS");
				LCDPrintfLine(2,"%.1f %.1f %.1f %.1f", adcA4, adcA5, adcA6, adcA7);
				break;
			case 2: 
				LCDPrintfLine(1,"USONIC READINGS");
				LCDPrintfLine(2,"%.1f %.1f", adcB4, adcB5);
				break;
			case 4: 
				LCDPrintfLine(1,"COMPASS");
				LCDPrintfLine(2,"%.1f", compass);
				break;
			case 7:
				LCDPrintfLine(1,"LADARA %.1f %.1f", LADARangle[1], LADARangle[225]);
				LCDPrintfLine(2,"%.1f %.1f %.1f", LADARangle[172], LADARangle[54], LADARangle[113]);
				break;
			case 8: 
				LCDPrintfLine(1,"GYRO READINGS");
				LCDPrintfLine(2,"400md: %.1f 100md: %.1f ", ((adcA2*3.0/4095.0)-1.23)*100*(PI/180.0), ((adcA3*3.0/4095.0)-1.23)*400*(PI/180.0));
				break;
			case 9: 
				LCDPrintfLine(1,"GYRO READINGS");
				LCDPrintfLine(2,"400md: %.1f 100md: %.1f ", ((adcA2*3.0/4095.0)-1.23)*100*(PI/180.0), ((adcA3*3.0/4095.0)-1.23)*400*(PI/180.0));
			break;
			case 10:
				LCDPrintfLine(1,"1pc: %.1f po: %.1f ", p_current1, p_old1);
				LCDPrintfLine(2,"2pc: %.1f po: %.1f ", p_current2, p_old2);
				break;
			case 11:
				LCDPrintfLine(1,"gv: %.1f gva: %.1f ", gyro_val, gyro_val_amp);
				LCDPrintfLine(2,"gg: %.1f", gyro_gain);
				break;
			case 12:
				LCDPrintfLine(1,"dummy: %d", dummy);
				break;
			case 13:
				LCDPrintfLine(1,"X: %.1f, Y: %.1f", pos_x, pos_y);
				LCDPrintfLine(2,"4: %.1f 1: %.1f ", angle_integral_amp, angle_integral);
				break;
			case 14:
				LCDPrintfLine(1,"frnterr: %.1f", front_wall_error);
				LCDPrintfLine(2,"thres: %.1f", front_error_threshold);			
				break;
			case 15:
				LCDPrintfLine(1,"turn: %.1f", turn);
				LCDPrintfLine(2,"vref: %.1f", vref);			
				break;
			default: // unknown
				LCDPrintfLine(1,"none selected");
				break;
		}
	}

}

pose UpdateOptitrackStates(pose localROBOTps, int * flag) {

	pose localOPTITRACKps;

	// Check for frame errors / packet loss
	if (previous_frame == Optitrackdata[OPTITRACKDATASIZE-1]) {
		frame_error++;
	}
	previous_frame = Optitrackdata[OPTITRACKDATASIZE-1];

	// Set local trackableID if first receive data
	if (firstdata){
		//trackableID = (int)Optitrackdata[OPTITRACKDATASIZE-1]; // removed to add new trackableID in shared memory
		trackableID = Optitrackdata[OPTITRACKDATASIZE-2];
		firstdata = 0;
	}

	// Check if local trackableID has changed - should never happen
	if (trackableID != Optitrackdata[OPTITRACKDATASIZE-2]) {
		trackableIDerror++;
		// do some sort of reset(?)
	}

	// Save position and yaw data
	if (isnan(Optitrackdata[0]) != 1) {  // this checks if the position data being received contains NaNs
		// check if x,y,yaw all equal 0.0 (almost certainly means the robot is untracked)
		if ((Optitrackdata[0] != 0.0) && (Optitrackdata[1] != 0.0) && (Optitrackdata[2] != 0.0)) {
			// save x,y
			// adding 2.5 so everything is shifted such that optitrack's origin is the center of the arena (while keeping all coordinates positive)
			localOPTITRACKps.x = Optitrackdata[0]*FEETINONEMETER; // was 2.5 for size = 5
			localOPTITRACKps.y = -1.0*Optitrackdata[1]*FEETINONEMETER+4.0;

			// make this a function
			temp_theta = fmodf(localROBOTps.theta,(float)(2*PI));//(theta[trackableID]%(2*PI));
			tempOPTITRACK_theta = Optitrackdata[2];
			if (temp_theta > 0) {
				if (temp_theta < PI) {
					if (tempOPTITRACK_theta >= 0.0) {
						// THETA > 0, kal in QI/II, OT in QI/II
						localOPTITRACKps.theta = ((int)((localROBOTps.theta)/(2*PI)))*2.0*PI + tempOPTITRACK_theta*2*PI/360.0;
					} else {
						if (temp_theta > (PI/2)) {
							// THETA > 0, kal in QII, OT in QIII
							localOPTITRACKps.theta = ((int)((localROBOTps.theta)/(2*PI)))*2.0*PI + PI + (PI + tempOPTITRACK_theta*2*PI/360.0);
						} else {
							// THETA > 0, kal in QI, OT in QIV
							localOPTITRACKps.theta = ((int)((localROBOTps.theta)/(2*PI)))*2.0*PI + tempOPTITRACK_theta*2*PI/360.0;
						}
					}
				} else {
					if (tempOPTITRACK_theta <= 0.0) {
						// THETA > 0, kal in QIII, OT in QIII
						localOPTITRACKps.theta = ((int)((localROBOTps.theta)/(2*PI)))*2.0*PI + PI + (PI + tempOPTITRACK_theta*2*PI/360.0);
					} else {
						if (temp_theta > (3*PI/2)) {
							// THETA > 0, kal in QIV, OT in QI
							localOPTITRACKps.theta = ((int)((localROBOTps.theta)/(2*PI)))*2.0*PI + 2*PI + tempOPTITRACK_theta*2*PI/360.0;
						} else {
							// THETA > 0, kal in QIII, OT in QII
							localOPTITRACKps.theta = (floorf((localROBOTps.theta)/((float)(2.0*PI))))*2.0*PI + tempOPTITRACK_theta*2*PI/360.0;
						}
					}
				}
			} else {
				if (temp_theta > -PI) {
					if (tempOPTITRACK_theta <= 0.0) {
						// THETA < 0, kal in QIII/IV, OT in QIII/IV
						localOPTITRACKps.theta = ((int)((localROBOTps.theta)/(2*PI)))*2.0*PI + tempOPTITRACK_theta*2*PI/360.0;
					} else {
						if (temp_theta < (-PI/2)) {
							// THETA < 0, kal in QIII, OT in QII
							localOPTITRACKps.theta = ((int)((localROBOTps.theta)/(2*PI)))*2.0*PI - PI + (-PI + tempOPTITRACK_theta*2*PI/360.0);
						} else {
							// THETA < 0, kal in QIV, OT in QI
							localOPTITRACKps.theta = ((int)((localROBOTps.theta)/(2*PI)))*2.0*PI + tempOPTITRACK_theta*2*PI/360.0;
						}
					}
				} else {
					if (tempOPTITRACK_theta >= 0.0) {
						// THETA < 0, kal in QI/II, OT in QI/II
						localOPTITRACKps.theta = ((int)((localROBOTps.theta)/(2*PI)))*2.0*PI - PI + (-PI + tempOPTITRACK_theta*2*PI/360.0);
					} else {
						if (temp_theta < (-3*PI/2)) {
							// THETA < 0, kal in QI, OT in QIV
							localOPTITRACKps.theta = ((int)((localROBOTps.theta)/(2*PI)))*2.0*PI - 2*PI + tempOPTITRACK_theta*2*PI/360.0;
						} else {
							// THETA < 0, kal in QII, OT in QIII
							localOPTITRACKps.theta = ((int)((localROBOTps.theta)/(2*PI)))*2.0*PI + tempOPTITRACK_theta*2*PI/360.0;
						}
					}
				}
			}
			*flag = 1;
		}
	}
	return localOPTITRACKps;
}
