#include <coecsl.h>
#include "i2c.h"
#include <user_includes.h>
#include <atmel_pwrboard2.h>

#define I2C_ACCESS_TIMEOUT 100
#define I2C_TIMEOUT 50

#define NUM_I2C_SEND_QUEUES 10
#define NUM_I2C_RECEIVE_QUEUES 20
#define MAX_I2C_SEND_LENGTH 30

int CompassNew;

unsigned char Compassbuff[4];
int I2Cbusy = 0;

typedef struct I2CSendStrQueObj {
	QUE_Elem	elem;
	char		msg[MAX_I2C_SEND_LENGTH];
	int			msg_len;
} I2CSendStrQueObj;

volatile char I2Ctxbuffer[64];
#pragma DATA_ALIGN(I2Ctxbuffer,64);

extern far Int L1SARAM;

int ir1_i2c=0,ir2_i2c=0,ir3_i2c=0,ir4_i2c=0,ir5_i2c=0;
int adc1_i2c=0,adc2_i2c=0,adc3_i2c=0,adc4_i2c=0,adc5_i2c=0,adc6_i2c=0,adc7_i2c=0,adc8_i2c=0;
unsigned char Atmel_RC1=0,Atmel_RC2=0,Atmel_RC3=0,Atmel_RC4=0,Atmel_RC5=0,Atmel_RC6=0;
volatile int atmelIRs_err = 0;
volatile int new_irdata_i2c = 0;


volatile int getIRs_err = 0;
volatile int getI2C_err = 0;

unsigned long task_state = 0;

extern unsigned long noi2c;
extern unsigned long timeint;




//---------------------------------------------------------------------------
// Example: InitI2CGpio: 
//---------------------------------------------------------------------------
// This function initializes GPIO pins to function as I2C pins
//
// Each GPIO pin can be configured as a GPIO pin or up to 3 different
// peripheral functional pins. By default all pins come up as GPIO
// inputs after reset.  
// 
// Caution: 
// Only one GPIO pin should be enabled for SDAA operation.
// Only one GPIO pin shoudl be enabled for SCLA operation. 
// Comment out other unwanted lines.

void InitI2CGpio(void)
{

   EALLOW;
/* Enable internal pull-up for the selected pins */
// Pull-ups can be enabled or disabled disabled by the user.  
// This will enable the pullups for the specified pins.
// Comment out other unwanted lines.

	GpioCtrlRegs.GPBPUD.bit.GPIO32 = 0;    // Enable pull-up for GPIO32 (SDAA)
	GpioCtrlRegs.GPBPUD.bit.GPIO33 = 0;	   // Enable pull-up for GPIO33 (SCLA)

/* Set qualification for selected pins to asynch only */
// This will select asynch (no qualification) for the selected pins.
// Comment out other unwanted lines.

	GpioCtrlRegs.GPBQSEL1.bit.GPIO32 = 3;  // Asynch input GPIO32 (SDAA)
    GpioCtrlRegs.GPBQSEL1.bit.GPIO33 = 3;  // Asynch input GPIO33 (SCLA)

/* Configure SCI pins using GPIO regs*/
// This specifies which of the possible GPIO pins will be I2C functional pins.
// Comment out other unwanted lines.

	GpioCtrlRegs.GPBMUX1.bit.GPIO32 = 1;   // Configure GPIO32 for SDAA operation
	GpioCtrlRegs.GPBMUX1.bit.GPIO33 = 1;   // Configure GPIO33 for SCLA operation
	
    EDIS;
}

/*void readCompassTSK(void) {

	volatile int CompassTemp=0;

	TSK_sleep(1000);
	while(1) {	
		TSK_sleep(20);
	    Compassbuff[0] = 'A';
	    i2c_stdsend(0x21,Compassbuff,1);
	    TSK_sleep(20);
	    i2c_stdrecv(0x21,0,Compassbuff,2);
	    CompassTemp = (((int) Compassbuff[0]) << 8) + Compassbuff[1];
	    CompassNew = CompassTemp;
	}
}*/

void Init_LCD(unsigned char contrast) {

	char sendmsg[40];

	sendmsg[0] = 0xFE;  // set contrast
	sendmsg[1] = 0x50;
	sendmsg[2] = (char) contrast;
	SendStr2_I2C(sendmsg,3);
	
	sendmsg[0] = 0xFE; // set Auto Line Wrap OFF
	sendmsg[1] = 0x44;
	SendStr2_I2C(sendmsg,2);
	
	sendmsg[0] = 0xFE; // set Auto Scroll OFF
	sendmsg[1] = 0x52;
	SendStr2_I2C(sendmsg,2);
	
	sendmsg[0] = 0xFE; // Backlight ON 
	sendmsg[1] = 0x42;
	sendmsg[2] = 0;
	SendStr2_I2C(sendmsg,3);
	
	sendmsg[0] = 0xFE; // Clear Display
	sendmsg[1] = 0x58;
	SendStr2_I2C(sendmsg,2);
	
	sendmsg[0] = 0xFE; // Cursor On
	sendmsg[1] = 0x4A;
	SendStr2_I2C(sendmsg,2);
		
}

void LCDvPrintfLine(unsigned char line, char *format, va_list ap)
{
	char sendmsg[24];
	char buffer[120];
	int i;

	vsprintf(buffer,format,ap);

	// Add header information and pad end of transfer with spaces to clear display
	sendmsg[0] = 0xFE;
	sendmsg[1] = 'G';
	sendmsg[2] = 1;
	sendmsg[3] = line;
	for (i=4;i<24;i++) {
		if (i >= strlen(buffer)+4) {
			sendmsg[i] = ' ';
		} else {
			sendmsg[i] = buffer[i-4];
		}
	}
	SendStr2_I2C(sendmsg,24);
}

void LCDPrintfLine(unsigned char line, char *format, ...)
{
	va_list ap;
    va_start(ap, format);
    LCDvPrintfLine(line,format,ap);
}

//  Function: void SendStr2UART1(char *buffer,int size)
//	Parameters:	
//		buffer		A pointer to the character array to be sent to UART 1.
//		size		The number of characters to send from the array buffer.
//	Return value:	None
//  Description:	Starts a task to send a string of characters to UART 1.
void SendStr2_I2C(char *buffer,int size) {
	
	int putserr = 0;
	I2CSendStrQueObj *I2Cmsg;
	int i;
	
	// Limit size of character transmission to MAX_SEND_LENGTH characters.
	if (size > MAX_I2C_SEND_LENGTH) size = MAX_I2C_SEND_LENGTH;
	if (size < 1) putserr = 1;

	if (putserr == 0) {
		// if the Que is full (i.e. freeQue empty) don't send the message... in the future should return an error
		if (!QUE_empty(&I2CSendStrfreeQueue) ) {
			I2Cmsg = QUE_get(&I2CSendStrfreeQueue);

			I2Cmsg->msg_len = size;
			for (i=0;i<size;i++) {
				I2Cmsg->msg[i] = buffer[i];
			}
			
			QUE_put(&I2CSendStrmsgQueue,I2Cmsg);
			SEM_post(&SEM_I2CSendStrmsg_rdy);
		}
	}
}

void I2Ctsk(void) {
	
	I2CSendStrQueObj *sendmsg;
	int i;
	int I2Ctxlength;
	unsigned long starttime;
	int i2cerror = 0;
	
	while(1) {  // loop forever
		SEM_pend(&SEM_I2CSendStrmsg_rdy,SYS_FOREVER);  // wait until LCD msg sent
		
		sendmsg = QUE_get(&I2CSendStrmsgQueue);
		I2Ctxlength = sendmsg->msg_len;
		for(i=0;i<I2Ctxlength;i++) {
			I2Ctxbuffer[i] = sendmsg->msg[i];
		}	

		starttime = CLK_getltime();
		i2cerror = 0;
		// Poll busy bit of I2C.  Loop until not busy.  Flag error and reset I2C if timeout.
		while ( (((I2caRegs.I2CSTR.all & 0x1000) == 0x1000) || (I2Cbusy == 1) ) && (i2cerror == 0) ){	
			TSK_yield();
			if ((CLK_getltime() - starttime) > I2C_ACCESS_TIMEOUT) {
				i2cerror = 1;
				I2caRegs.I2CSTR.all = 0x1000; 		    
				I2caRegs.I2CMDR.all = 0x0; // reset I2C
			}

		}
			
		if (i2cerror == 0) {
			I2Cbusy = 1;
			
			I2caRegs.I2CMDR.all = 0x0;              // reset I2C
			I2caRegs.I2CCNT = I2Ctxlength;          // set number of data bytes  
			I2caRegs.I2CSAR = 0x28;                 // set LCD address (0x28 << 1) = 0x50
			I2caRegs.I2CMDR.all = 0x2E20;           // 8 bits/data byte, free data format disabled, START byte disabled,
			                                        // I2C module enabled, DLB disabled, nonrepeat mode,
			                                        // 7-bit addressing, transmitter mode, MST, STP condition,
			                                        // STT condition

			// send data
			for (i = 0; i < I2Ctxlength; i++) {
				starttime = CLK_getltime();
				I2caRegs.I2CDXR = I2Ctxbuffer[i];   // Trasmit data shifted to I2CXSR immediately
		
				// Wait for XRDY bit to equal 1.  Flag error and reset I2C if timeout.
				while ( ((I2caRegs.I2CSTR.all & 0x10) == 0x0) && (i2cerror == 0) ) {
					TSK_yield();
					if ( (CLK_getltime() - starttime) > I2C_TIMEOUT ) {
						I2caRegs.I2CSTR.all = 0x1000; 		    
						I2caRegs.I2CMDR.all = 0x0; // reset I2C
						I2Cbusy = 0;
						i2cerror = 1;
					}
				}
			}
			
			/*if (SEM_pend(&SEM_I2CSendStrmsg_done,10*I2Ctxlength) == FALSE)  {
				I2caRegs.I2CSTR.all = 0x1000; 		    
			    I2caRegs.I2CMDR.all = 0x0; // reset I2C
			}*/
		}
		
		I2Cbusy = 0;
		TSK_sleep(1);// add a delay			
		
		QUE_put(&I2CSendStrfreeQueue,sendmsg);
	}
}


int i2c_stdsend(int I2Caddress, unsigned char *buf, int length){
	
	unsigned long starttime;
	int i;

	// Wait for Busy Bit.  Reset I2C if timed out.
	starttime = CLK_getltime();
	while ( ((I2caRegs.I2CSTR.all & 0x1000) == 0x1000) || (I2Cbusy == 1) ){
		TSK_yield();
		if ( (CLK_getltime() - starttime) > I2C_ACCESS_TIMEOUT ) {
			I2caRegs.I2CSTR.all = 0x1000; 		    
	    	I2caRegs.I2CMDR.all = 0x0; // reset I2C
	    	return 1;
		}
	}

	// setup i2c
	I2Cbusy = 1;
	I2caRegs.I2CMDR.all = 0x0; // reset I2C
	I2caRegs.I2CSAR = I2Caddress;
	I2caRegs.I2CCNT = length;
	I2caRegs.I2CMDR.all = 0x2E20;           // 8 bits/data byte, free data format disabled, START byte disabled,
			                                // I2C module enabled, DLB disabled, nonrepeat mode,
			                                // 7-bit addressing, transmitter mode, MST, STP condition,
			                                // STT condition

	// send data
	for (i = 0; i < length; i++) {
		starttime = CLK_getltime();
		I2caRegs.I2CDXR = buf[i];			// Trasmit data shifted to I2CXSR immediately

		// Wait for XRDY bit to equal 1.  Flag not busy and return.
		while ( (I2caRegs.I2CSTR.all & 0x10) == 0x0 ) {
			TSK_yield();
			if ( (CLK_getltime() - starttime) > I2C_TIMEOUT ) {
				I2caRegs.I2CSTR.all = 0x1000; 		    
				I2caRegs.I2CMDR.all = 0x0; // reset I2C
				I2Cbusy = 0;
				return 1;
			}
		}
	}
	I2Cbusy = 0;
	return 0;
}

int i2c_stdrecv(int I2Caddress, unsigned char addr, unsigned char *buf, int length)
{
	unsigned long starttime;
	int i;

	// Wait for Busy Bit.  Reset I2C if timed out.
	starttime = CLK_getltime();
	while ( ((I2caRegs.I2CSTR.all & 0x1000) == 0x1000) || (I2Cbusy == 1) ){
		TSK_yield();
		if ( (CLK_getltime() - starttime) > I2C_ACCESS_TIMEOUT ) {
			I2caRegs.I2CSTR.all = 0x1000; 		    
	    	I2caRegs.I2CMDR.all = 0x0; // reset I2C
	    	return 1;
		}
	}

	// send address byte
	I2Cbusy = 1;
	I2caRegs.I2CMDR.all = 0x0; 		// reset I2C
	I2caRegs.I2CCNT = 0x1;  		// Receive one data byte
	I2caRegs.I2CSAR = I2Caddress;	// Set address
	I2caRegs.I2CMDR.all = 0x2620;	// 8 bits/data byte, free data format disabled, START byte disabled,
			                        // I2C module enabled, DLB disabled, nonrepeat mode,
			                        // 7-bit addressing, transmitter mode, MST, STP condition auto cleared,
			                        // STT condition
	I2caRegs.I2CDXR = addr;			// Set transmit register

	starttime = CLK_getltime();
	while ( (I2caRegs.I2CSTR.all & 0x10) == 0x0 ) {
		TSK_yield();
		if ((CLK_getltime() - starttime) > I2C_TIMEOUT) {
			I2caRegs.I2CSTR.all = 0x1000; 		    
			I2caRegs.I2CMDR.all = 0x0; // reset I2C
			I2Cbusy = 0;
			return 1;
		}
	}

	// recieve data
	I2caRegs.I2CCNT = length;
	I2caRegs.I2CMDR.all = 0x2C20;	// 8 bits/data byte, free data format disabled, START byte disabled,
			                        // I2C module enabled, DLB disabled, nonrepeat mode,
			                        // 7-bit addressing, receiver mode, MST, STP condition,
			                        // STT condition

	for (i = 0; i < length; i++) {
		starttime = CLK_getltime();
		// Wait for RRDY bit to equal 1 then read register
		while ( (I2caRegs.I2CSTR.all & 0x08) == 0x0 ) {
			TSK_yield();
			if ( (CLK_getltime() - starttime) > I2C_TIMEOUT ) {
				I2caRegs.I2CSTR.all = 0x1000; 		    
				I2caRegs.I2CMDR.all = 0x0; // reset I2C
				I2Cbusy = 0;
				return 1;
			}
		}
		buf[i] = I2caRegs.I2CDRR;
	}
	I2Cbusy = 0;
	return 0;
}


// initial I2C0 to communicate at 100Kbit per second
void Init_i2c(void) {
	
	I2CSendStrQueObj	*SendStr_que_setup;
	int i;
	
	// I2C code
	I2caRegs.I2CMDR.all = 0x0;  	// put in reset state
	I2caRegs.I2COAR = 0x0; 			// Master device so no address
	I2caRegs.I2CIER.all = 0x0;  	// No Interrupts
	I2caRegs.I2CSTR.all = 0x301F; 	// clear all possible ints and stats
	while( (I2caRegs.I2CSTR.all & 0x1000) == 0x1000) {
    	I2caRegs.I2CSTR.all = 0x1000;
    }
    I2caRegs.I2CPSC.all = 0x0;		// No prescaler
	I2caRegs.I2CCLKL = 0x400;		// Set SCL to ~70kHz
	I2caRegs.I2CCLKH = 0x400;
	I2caRegs.I2CCNT = 0x0;          // Init count to zero
	
	// setup Ques in L1SARAM and initial LCD messages	
	SendStr_que_setup = (I2CSendStrQueObj *)MEM_alloc(L1SARAM,NUM_I2C_SEND_QUEUES * sizeof(I2CSendStrQueObj),0);

	if (SendStr_que_setup == MEM_ILLEGAL) {
		SYS_abort("Memory allocation failed!\n");
	}
	
	// put all allocated memory into the "free" Queue
	for (i=0;i < NUM_I2C_SEND_QUEUES;i++,SendStr_que_setup++) {
		QUE_put(&I2CSendStrfreeQueue,SendStr_que_setup); 
	}
}

// Atmel I2C interface (board version >=2.1)
int atmel_set_rcservo(int first, int count, unsigned char *buf)
{
	unsigned char outbuf[ATPWR_SERVOS+1];
	int i;

	outbuf[0] = ATPWR_SET_SERVO | (first & 0x0F);
	for (i = 0; i < count; i++) {
		outbuf[i+1] = buf[i];
	}
	return i2c_stdsend(ATPWR_I2CADDRESS,outbuf,count+1);
}

int atmel_get_ir(int first, int count, unsigned char *buf)
{
	if (i2c_stdrecv(ATPWR_I2CADDRESS,(ATPWR_GET_IR | (first & 0x0F)),buf,count)) return 1;
	return 0;
}

int atmel_get_adc(int first, int count, unsigned char *buf)
{
	if (i2c_stdrecv(ATPWR_I2CADDRESS,(ATPWR_GET_ADC | (first & 0x0F)),buf,count)) return 1;
	return 0;
}

int atmel_get_all(unsigned char *buf)
{
	if (i2c_stdrecv(ATPWR_I2CADDRESS,ATPWR_GET_ALL,buf,ATPWR_ALL_DATA)) return 1;
	return 0;
}

int atmel_get_version(unsigned char *buf)
{
	return i2c_stdrecv(ATPWR_I2CADDRESS,ATPWR_GET_VERSION,buf,1);
}

int atmel_set_mode(unsigned char mode)
{
	mode = ATPWR_SET_MODE | (mode & 0x0F);
	return i2c_stdsend(ATPWR_I2CADDRESS,&mode,1);
}

/*void atmel_IRs_task(void)
{
	unsigned char IRbuf[15];
	
	TSK_sleep(1000); // give I2C bus some intial rest time.  

	while (1) {
        if (new_irdata_i2c == 0) {
            if ( atmel_get_all(IRbuf) == 0 ) {
                ir1_i2c = IRbuf[0];
                ir2_i2c = IRbuf[1];
                ir3_i2c = IRbuf[2];
                ir4_i2c = IRbuf[3];
                ir5_i2c = IRbuf[4];
                adc1_i2c = IRbuf[5];
                adc2_i2c = IRbuf[6];
                adc3_i2c = IRbuf[7];
                adc4_i2c = IRbuf[8];
                adc5_i2c = IRbuf[9];
                adc6_i2c = IRbuf[10];
                adc7_i2c = IRbuf[11];
                adc8_i2c = IRbuf[12];
                new_irdata_i2c = 1;
                noi2c = timeint;
                TSK_sleep(70);
            } else {
                TSK_sleep(5);
            }
        } else {
			atmelIRs_err++;
            TSK_sleep(70);  // error condition, should only get here when control loop is not looking for new_irdata_i2c = 1
        }
	}
}*/

// Set the global variables  Atmel_RC1 and Atmel_RC2 to a value between 0 and 255 and then post the Semaphore
//  SEM_atmel_rcservo.  You will have to create the SEM_atmel_rcservo in DSP/BIOS 
// Also add this task in DSP/BIOS
// In vision segmentation project creator it is aready added and semaphore created.
/*void atmel_RCservo_task(void)
{	
    // set all to default of 128
	unsigned char RCbuf[14] = {128,128,128,128,128,128,128,128,128,128,128,128,128,128};
	
    atmel_set_mode(ATPWR_MODE_ADCS);  // The firmware on the Atmel only supports the ADC mode.  So only the last 6 RCservos starting with 8
    
    TSK_sleep(500); // give I2C bus some intial rest time.  
	
	while (1) {
		SEM_pend(&SEM_atmel_rcservo,SYS_FOREVER);

		RCbuf[0] = Atmel_RC1;
		RCbuf[1] = Atmel_RC2;
        RCbuf[2] = Atmel_RC3;
        RCbuf[3] = Atmel_RC4;
		RCbuf[4] = Atmel_RC5;
        RCbuf[5] = Atmel_RC6;
        
        // Could send more servos here  only using last 6 servos
		atmel_set_rcservo(8, 6, RCbuf);
        
		TSK_sleep(50); // wait 50ms before looking for next value set 
	}

}*/

void atmelcompass(void) {

	volatile int CompassTemp=0;

	TSK_sleep(1000);


	// Compass Calibration code - on the OMAP, make the robot rotate slowly in place
	/*GpioDataRegs.GPASET.bit.GPIO31 = 1;
	GpioDataRegs.GPBSET.bit.GPIO34 = 1;
	CompassNew = timeint;
	Compassbuff[0] = 'C';
	i2c_stdsend(0x21,Compassbuff,1);
	while((timeint - CompassNew)<60000){}
	Compassbuff[0] = 'E';
	i2c_stdsend(0x21,Compassbuff,1);
	GpioDataRegs.GPATOGGLE.bit.GPIO31 = 1;
	GpioDataRegs.GPBTOGGLE.bit.GPIO34 = 1;

	TSK_sleep(100);*/


	while (1) {
		//GpioDataRegs.GPATOGGLE.bit.GPIO31 = 1;
		//GpioDataRegs.GPASET.bit.GPIO31 = 1;
		if ((task_state%10)==0){
			//compass send or receive
			if ((task_state%20)==0){
				i2c_stdrecv(0x21,0,Compassbuff,2);
				CompassTemp = (((int) Compassbuff[0]) << 8) + Compassbuff[1];
				CompassNew = CompassTemp;
			} else {
				Compassbuff[0] = 'A';
				i2c_stdsend(0x21,Compassbuff,1);
			}
		}

		//GpioDataRegs.GPATOGGLE.bit.GPIO31 = 1;
		task_state++;
		TSK_sleep(10);
	}

}


