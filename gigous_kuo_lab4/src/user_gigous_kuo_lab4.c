#include <tistdtypes.h>
#include <coecsl.h>
#include "user_includes.h"
#include "28335_dma.h"
#include "28335_spi.h"
#include "28335_inits.h"
#include "mcbsp_com.h"
#include "i2c.h"

void updateData(void);
void sendData(void);

extern volatile int new_irdata_i2c;
extern int adc1_i2c;
extern int adc2_i2c;
extern int adc3_i2c;
extern int adc4_i2c;
extern int adc5_i2c;
extern int adc6_i2c;
extern int adc7_i2c;
extern int adc8_i2c;
extern int CompassNew;
extern int ir1_i2c,ir2_i2c,ir3_i2c,ir4_i2c,ir5_i2c;

extern int SPIenc_state;
extern long SPIenc1_reading;
extern long SPIenc2_reading;
extern long SPIenc3_reading;
extern long SPIenc4_reading;

extern unsigned int nocomm;
extern int newOMAPdata;
extern int PIVEL_enable;
extern float mcbsp_vref;
extern float mcbsp_turn;
extern float mcbsp_pwm1;
extern float mcbsp_pwm2;
extern float mcbsp_dac1;
extern float mcbsp_dac2;
extern long McBSP_COMerr;
extern int McBSP_RecGoodData_ReadyTX;
extern long McBSPb_int_count;
extern int McBSPB_rdy;
extern mcbsp28x_com TXBuff;
extern mcbspL138_com RXBuff;

unsigned long timeint = 0;
unsigned long noi2c = 0;

float gyro_x = 0;
float gyro_y = 0;

float Enc1_rad = 0;
float Enc2_rad = 0;
float Enc3_rad = 0;
float Enc4_rad = 0;

int switchstate = -2;

int newF28335_Extra = 0;
float F28335_Extra1 = 0.0;
float F28335_Extra2 = 0.0;
float F28335_Extra3 = 0.0;
float F28335_Extra4 = 0.0;

int raw_adc_A0 = 0;
int raw_adc_A1 = 0;
int raw_adc_A2 = 0;
int raw_adc_A3 = 0;
int raw_adc_A4 = 0;
int raw_adc_A5 = 0;
int raw_adc_A6 = 0;
int raw_adc_A7 = 0;
int raw_adc_B0 = 0;
int raw_adc_B1 = 0;
int raw_adc_B2 = 0;
int raw_adc_B3 = 0;
int raw_adc_B4 = 0;
int raw_adc_B5 = 0;
int raw_adc_B6 = 0;
int raw_adc_B7 = 0;

float float_adc_A0 = 0;
float float_adc_A1 = 0;
float float_adc_A2 = 0;
float float_adc_A3 = 0;
float float_adc_A4 = 0;
float float_adc_A5 = 0;
float float_adc_A6 = 0;
float float_adc_A7 = 0;
float float_adc_B0 = 0;
float float_adc_B1 = 0;
float float_adc_B2 = 0;
float float_adc_B3 = 0;
float float_adc_B4 = 0;
float float_adc_B5 = 0;
float float_adc_B6 = 0;
float float_adc_B7 = 0;

float omap_vref = 0;
float omap_turn = 0;
float omap_pwm1 = 0;
float omap_pwm2 = 0;
float omap_dac1 = 0;
float omap_dac2 = 0;

eqep_t enc1;
eqep_t enc2;
float value_enc1 = 0;
float value_enc2 = 0;


void main(void)
{

	//  Init and zero encoders
	init_EQEP(&enc1, EQEP1, 3000, 1, 0.0);
	init_EQEP(&enc2, EQEP2, 3000, -1, 0.0);
	EQep1Regs.QPOSCNT = 0;
	EQep2Regs.QPOSCNT = 0;

	// Initialize PWMs
	init_PWM(EPWM1);
	init_PWM(EPWM2);
	init_PWM_AS_RCSERVO(EPWM3B);  // sets up EPWM3A and EPWM3B for RCservo
	init_PWM_AS_RCSERVO(EPWM4B); // sets up EPWM4A and EPWM4B for RCservo
	init_PWM_AS_RCSERVO(EPWM5);  // sets up EPWM5A for RCservo

	// System initializations
	pre_init();

	// initialize serial port A to 115200 baud
	init_serial(&SerialA,115200,NULL);
	// initialize serial port B to 57600 baud
	init_serial(&SerialB,57600,NULL);
	// initialize serial port C to 19200 baud
	init_serial(&SerialC,19200,NULL);

	EALLOW;  // set up LED GPIOs
		GpioCtrlRegs.GPAMUX2.bit.GPIO30 = 0;
		GpioDataRegs.GPACLEAR.bit.GPIO30 = 1;
		GpioCtrlRegs.GPADIR.bit.GPIO30 = 1;
		GpioCtrlRegs.GPAMUX2.bit.GPIO31 = 0;
		GpioDataRegs.GPACLEAR.bit.GPIO31 = 1;
		GpioCtrlRegs.GPADIR.bit.GPIO31 = 1;
		GpioCtrlRegs.GPBMUX1.bit.GPIO34 = 0;
		GpioDataRegs.GPBSET.bit.GPIO34 = 1;
		GpioCtrlRegs.GPBDIR.bit.GPIO34 = 1;

		// set up GPIO3 for amp enable or disable
		GpioCtrlRegs.GPAMUX1.bit.GPIO3 = 0;
		GpioDataRegs.GPACLEAR.bit.GPIO3 = 1;
		GpioCtrlRegs.GPADIR.bit.GPIO3 = 1;

	EDIS; 

	init_dma_mcbsp( (Uint32)&TXBuff.darray[0],(Uint32)&RXBuff.darray[0]);
	InitMcbspbGpio();
	InitMcbspb();
	InitMcbspb32bit();
	init_SPI();
	InitI2CGpio();
	Init_i2c();
	
	// Add your inits here

	InitAdc(); // This function takes care of powering on the ADC correctly.
	AdcRegs.ADCTRL1.bit.SUSMOD = 0; // Emulation Suspend mode ignored
	AdcRegs.ADCTRL1.bit.ACQ_PS = 4; // 5 ADCCLK periods
	AdcRegs.ADCTRL1.bit.CPS = 0; // divide by 1
	AdcRegs.ADCTRL1.bit.CONT_RUN = 0; // Start Stop mode
	AdcRegs.ADCTRL1.bit.SEQ_OVRD = 0; //Sequencer Override Disabled
	AdcRegs.ADCTRL1.bit.SEQ_CASC = 1; // Cascaded mode enabled
	AdcRegs.ADCTRL2.bit.INT_ENA_SEQ1 = 1; //Enable SEQ1’s interrupt
	AdcRegs.ADCTRL2.bit.INT_MOD_SEQ1 = 0; //Set Interrupt to occur every SEQ1 completion
	AdcRegs.ADCTRL3.bit.ADCCLKPS = 9; //Core clock divided by 18
	// Found this by trial and error. Could need to be changed
	// given different sensor inputs
	AdcRegs.ADCTRL3.bit.SMODE_SEL = 1; //Set to Simultaneous sampling mode
	AdcRegs.ADCMAXCONV.bit.MAX_CONV1 = 4; // set so that A0 BO, A1 B1, A2 B2, A3 B3, A4 B4 are
	 // sampled
	AdcRegs.ADCREFSEL.bit.REF_SEL = 0; // Internal reference used
	AdcRegs.ADCCHSELSEQ1.bit.CONV00 = 0x0; // set these 5 fields so that the order of ADC channels
	AdcRegs.ADCCHSELSEQ1.bit.CONV01 = 0x1; // sampled is A0,B0,A1,B1,A2,B2,A3,B3,A4,B4.
	AdcRegs.ADCCHSELSEQ1.bit.CONV02 = 0x2;
	AdcRegs.ADCCHSELSEQ1.bit.CONV03 = 0x3;
	AdcRegs.ADCCHSELSEQ2.bit.CONV04 = 0x4;
	AdcRegs.ADCTRL2.bit.RST_SEQ1 = 1; // Reset SEQ1
	AdcRegs.ADCST.bit.INT_SEQ1_CLR = 0; // Clear INT SEQ1 Good practice to clear possible
	 // interrupt before enabling the interrupt.

	PieCtrlRegs.PIEACK.all = PIEACK_GROUP1; // Clear possible interrupts before enabling int.
	PieCtrlRegs.PIEIER1.bit.INTx6 = 1; // enable PIE interrupt 1.6
	IFR &= ~M_INT1; // Clear a possible pending interrupt in the interrupt flag register
	IER |= M_INT1; // Enable legacy INT1 in the interrupt enable register

	// Finalize inits and start DMA/McBSP
	post_init();

}

float B[8]={	1.0/7.0,	1.0/7.0,	1.0/7.0,	1.0/7.0,	1.0/7.0,	1.0/7.0,	1.0/7.0, 1.0/7.0};
float A[8]={	1.0, 0.0, 0.0,0.0,0.0,0.0,0.0,0.0};

float b[8]={	1.2296498873e-06,	8.6075492111e-06,	2.5822647633e-05,	4.3037746056e-05,	4.3037746056e-05,	2.5822647633e-05,	8.6075492111e-06,	1.2296498873e-06};
float a[8]={	1.0000000000e+00,	-5.5889821399e+00,	1.3504744700e+01,	-1.8270852518e+01,	1.4936521932e+01,	-7.3738011769e+00,	2.0343997965e+00,	-2.4187319898e-01};

float x[8]={	0,0,0,0,0,0,0,0 };
float y[8]={	0,0,0,0,0,0,0,0 };

void start_dataCollection(void) {

	// Start SPI
//	start_SPI();

	//new instruction we start data collection
	AdcRegs.ADCTRL2.bit.SOC_SEQ1 = 1;

}

void adc_ISR(void) {

	raw_adc_A0 = AdcMirror.ADCRESULT0;
	raw_adc_B0 = AdcMirror.ADCRESULT1;
	raw_adc_A1 = AdcMirror.ADCRESULT2;
	raw_adc_B1 = AdcMirror.ADCRESULT3;
	raw_adc_A2 = AdcMirror.ADCRESULT4;
	raw_adc_B2 = AdcMirror.ADCRESULT5;
	raw_adc_A3 = AdcMirror.ADCRESULT6;
	raw_adc_B3 = AdcMirror.ADCRESULT7;
	raw_adc_A4 = AdcMirror.ADCRESULT8;
	raw_adc_B4 = AdcMirror.ADCRESULT9;

	start_SPI();

	// Reinitialize for next ADC sequence
	AdcRegs.ADCTRL2.bit.RST_SEQ1 = 1; // Reset SEQ1
	AdcRegs.ADCST.bit.INT_SEQ1_CLR = 1; // Clear INT SEQ1 bit
	PieCtrlRegs.PIEACK.all=PIEACK_GROUP1;//Acknowledge/CLR interrupt to PIE

}

void control(void) {

	updateData();

	// Add your code here in between updateData and sendData

	float_adc_B1 = (raw_adc_B1/4095.0)*3.0;
	float_adc_B2 = (raw_adc_B2/4095.0)*3.0;

	F28335_Extra1 = float_adc_B1;
	F28335_Extra2 = float_adc_B2;

	float dac_adc_B1 = 0;
	float dac_adc_B2 = 0;

	if (float_adc_B1 > 2.5)
		dac_adc_B1 = 2.5;
	else
		dac_adc_B1 = float_adc_B1;

	if (float_adc_B2 > 2.5)
		dac_adc_B2 = 2.5;
	else
		dac_adc_B2 = float_adc_B2;

	x[0] = dac_adc_B1;

	y[0] = 0;

	int i;
	for (i=0; i<8; i++ )
	{
		y[0] += b[i]*x[i];
	}
	for ( i=1; i<8; i++ )
	{
		y[0] -= a[i]*y[i];
	}

	//writeDAC7564(y[0], dac_adc_B2);
	writeDAC7564(omap_dac1, omap_dac2);

	for ( i=7; i>0; i-- )
	{
		x[i] = x[i-1];
		y[i] = y[i-1];
	}



	sendData();


	timeint++;

}

void updateData(void) {

	value_enc1 = EQEP_read(&enc1);
	value_enc2 = EQEP_read(&enc2);

	Enc1_rad = ((float) SPIenc1_reading)*(TWOPI/(2000.0f)); // left encoder
	Enc1_rad = -Enc1_rad;
	Enc2_rad = ((float) SPIenc2_reading)*(TWOPI/(2000.0f)); // right encoder
	Enc3_rad = ((float) SPIenc3_reading)*(TWOPI/(2000.0f));
	Enc4_rad = ((float) SPIenc4_reading)*(TWOPI/(2000.0f));

	
	switchstate = (int)((0x000F & ~(((GpioDataRegs.GPBDAT.all & 0x0C000000)>>24) | ((GpioDataRegs.GPBDAT.all & 0x00030000)>>16))));

	if (newOMAPdata) {
		omap_vref = mcbsp_vref;
		omap_turn = mcbsp_turn;
		omap_pwm1 = mcbsp_pwm1;
		omap_pwm2 = mcbsp_pwm2;
		omap_dac1 = mcbsp_dac1;
		omap_dac2 = mcbsp_dac2;
		newOMAPdata = 0;
	}

}

void sendData(void) {

	if (McBSPB_rdy == 1) {
		TXBuff.data.validcode = VALID_28XTO67X;
		TXBuff.data.DSC_ENC[0] = value_enc1;
		TXBuff.data.DSC_ENC[1] = value_enc2;
		TXBuff.data.SPI_ENC[0] = (float) Enc1_rad;
		TXBuff.data.SPI_ENC[1] = (float) Enc2_rad;
		TXBuff.data.SPI_ENC[2] = (float) Enc3_rad;
		TXBuff.data.SPI_ENC[3] = (float) Enc4_rad;
		TXBuff.data.compass = (float)CompassNew;
		TXBuff.data.switchstate = (float)switchstate;
		TXBuff.data.ADC[0] = (float)raw_adc_A0;
		TXBuff.data.ADC[1] = (float)raw_adc_B0;
		TXBuff.data.ADC[2] = (float)raw_adc_A1;
		TXBuff.data.ADC[3] = (float)raw_adc_B1;
		TXBuff.data.ADC[4] = (float)raw_adc_A2;
		TXBuff.data.ADC[5] = (float)raw_adc_B2;
		TXBuff.data.ADC[6] = (float)raw_adc_A3;
		TXBuff.data.ADC[7] = (float)raw_adc_B3;
		TXBuff.data.ADC[8] = (float)raw_adc_A4;
		TXBuff.data.ADC[9] = (float)raw_adc_B4;
		TXBuff.data.ADC[10] = (float)raw_adc_A5;
		TXBuff.data.ADC[11] = (float)raw_adc_B5;
		TXBuff.data.ADC[12] = (float)raw_adc_A6;
		TXBuff.data.ADC[13] = (float)raw_adc_A7;
		TXBuff.data.ADC[14] = 0; // Reserved   was (float)atmel_adc1;
		TXBuff.data.ADC[15] = 0; // Reserved   was (float)atmel_adc2;
		TXBuff.data.ADC[16] = 0; // Reserved   was (float)atmel_adc3;
		TXBuff.data.ADC[17] = 0; // Reserved   was (float)atmel_adc4;
		TXBuff.data.IR[0] = 0; // Reserved   was (float)atmel_ir1;
		TXBuff.data.IR[1] = 0; // Reserved   was (float)atmel_ir2;
		TXBuff.data.IR[2] = 0; // Reserved   was (float)atmel_ir3;
		TXBuff.data.IR[3] = 0; // Reserved   was (float)atmel_ir4;
		TXBuff.data.IR[4] = 0; // Reserved   was (float)atmel_ir5;
		TXBuff.data.GYRO[0] = (float)gyro_x;
		TXBuff.data.GYRO[1] = (float)gyro_y;
		TXBuff.data.F28335_EXTRA[0] = (float)F28335_Extra1;
		TXBuff.data.F28335_EXTRA[1] = (float)F28335_Extra2;
		TXBuff.data.F28335_EXTRA[2] = (float)F28335_Extra3;
		TXBuff.data.F28335_EXTRA[3] = (float)F28335_Extra4;

		if (McBSP_RecGoodData_ReadyTX == 1) {
			McBSPB_rdy = 0;
			start_dma();
			McBSP_RecGoodData_ReadyTX = 0;
		}
	}
}

