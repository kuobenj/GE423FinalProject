
#ifndef __ATMEL_PWRBOARD2_H__
#define __ATMEL_PWRBOARD2_H__

#define ATPWR_I2CADDRESS	0x55
#define ATPWR_VERSION		0x02
                          
#define ATPWR_SERVOS_SET1	8
#define ATPWR_SERVOS_SET2	6

#define ATPWR_SERVOS		(ATPWR_SERVOS_SET1+ATPWR_SERVOS_SET2)
#define ATPWR_IRS	  		5
#define ATPWR_ADCS	  		8
#define ATPWR_INTS			2
#define ATPWR_ALL_DATA		(ATPWR_IRS+ATPWR_ADCS+ATPWR_INTS)

// commands (top 4-bits, 0xF0)
#define ATPWR_GET_VERSION	0x00	// no parameters
#define ATPWR_SET_MODE		0x10	// parameter: mode
#define ATPWR_SET_SERVO		0x20	// parameter: servo#
#define ATPWR_GET_IR		0x30	// parameter: ir#
#define ATPWR_GET_ADC		0x40	// parameter: adc#
#define ATPWR_GET_INT		0x50	// parameter: int#
#define ATPWR_GET_ALL		0x60	// reads all IR, ADC, INT
#define ATPWR_SET_UART		0x70	// parameter: uartbuff#

// ATPWR_SET_MODE parameters (bottom 4-bits, 0x0F)
#define ATPWR_MODE_SERVOS	0x01
#define ATPWR_MODE_ADCS		0x02

#endif /* __ATMEL_PWRBOARD2_H__ */
