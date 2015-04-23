################################################################################
# Automatically-generated file. Do not edit!
################################################################################

# Add inputs and outputs from these tool invocations to the build variables 
CMD_SRCS += \
C:/gigous2_bkuo/repo/Lab6F28335Project/user_lnk.cmd 

TCF_SRCS += \
C:/gigous2_bkuo/repo/Lab6F28335Project/DSPBIOS/Lab6F28335Project.tcf 

ASM_SRCS += \
C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_common/source/DSP2833x_ADC_cal.asm \
C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_common/source/DSP2833x_CSMPasswords.asm \
C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_common/source/DSP2833x_CodeStartBranch.asm \
C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_common/source/DSP2833x_usDelay.asm 

S??_SRCS += \
./Lab6F28335Projectcfg.s?? 

C_SRCS += \
C:/gigous2_bkuo/repo/Lab6F28335Project/src/28335_dma.c \
C:/gigous2_bkuo/repo/Lab6F28335Project/src/28335_ecan.c \
C:/gigous2_bkuo/repo/Lab6F28335Project/src/28335_eqep.c \
C:/gigous2_bkuo/repo/Lab6F28335Project/src/28335_inits.c \
C:/gigous2_bkuo/repo/Lab6F28335Project/src/28335_mcbsp.c \
C:/gigous2_bkuo/repo/Lab6F28335Project/src/28335_pwm.c \
C:/gigous2_bkuo/repo/Lab6F28335Project/src/28335_serial.c \
C:/gigous2_bkuo/repo/Lab6F28335Project/src/28335_spi.c \
C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_common/source/DSP2833x_Adc.c \
C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_common/source/DSP2833x_CpuTimers.c \
C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_headers/source/DSP2833x_GlobalVariableDefs.c \
C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_common/source/DSP2833x_Mcbsp.c \
C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_common/source/DSP2833x_PieCtrl.c \
C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_common/source/DSP2833x_Spi.c \
C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_common/source/DSP2833x_SysCtrl.c \
./Lab6F28335Projectcfg_c.c \
C:/gigous2_bkuo/repo/Lab6F28335Project/src/RTDX_functions.c \
C:/gigous2_bkuo/repo/Lab6F28335Project/src/coecsl.c \
C:/gigous2_bkuo/repo/Lab6F28335Project/src/i2c.c \
C:/gigous2_bkuo/repo/Lab6F28335Project/src/lcd.c \
C:/gigous2_bkuo/repo/Lab6F28335Project/src/smallprintf.c \
C:/gigous2_bkuo/repo/Lab6F28335Project/src/user_Lab6F28335Project.c \
C:/gigous2_bkuo/repo/Lab6F28335Project/src/user_PIFuncs.c 

OBJS += \
./28335_dma.obj \
./28335_ecan.obj \
./28335_eqep.obj \
./28335_inits.obj \
./28335_mcbsp.obj \
./28335_pwm.obj \
./28335_serial.obj \
./28335_spi.obj \
./DSP2833x_ADC_cal.obj \
./DSP2833x_Adc.obj \
./DSP2833x_CSMPasswords.obj \
./DSP2833x_CodeStartBranch.obj \
./DSP2833x_CpuTimers.obj \
./DSP2833x_GlobalVariableDefs.obj \
./DSP2833x_Mcbsp.obj \
./DSP2833x_PieCtrl.obj \
./DSP2833x_Spi.obj \
./DSP2833x_SysCtrl.obj \
./DSP2833x_usDelay.obj \
./Lab6F28335Projectcfg.obj \
./Lab6F28335Projectcfg_c.obj \
./RTDX_functions.obj \
./coecsl.obj \
./i2c.obj \
./lcd.obj \
./smallprintf.obj \
./user_Lab6F28335Project.obj \
./user_PIFuncs.obj 

GEN_MISC_FILES += \
./Lab6F28335Project.cdb 

ASM_DEPS += \
./DSP2833x_ADC_cal.pp \
./DSP2833x_CSMPasswords.pp \
./DSP2833x_CodeStartBranch.pp \
./DSP2833x_usDelay.pp 

GEN_HDRS += \
./Lab6F28335Projectcfg.h \
./Lab6F28335Projectcfg.h?? 

S??_DEPS += \
./Lab6F28335Projectcfg.pp 

C_DEPS += \
./28335_dma.pp \
./28335_ecan.pp \
./28335_eqep.pp \
./28335_inits.pp \
./28335_mcbsp.pp \
./28335_pwm.pp \
./28335_serial.pp \
./28335_spi.pp \
./DSP2833x_Adc.pp \
./DSP2833x_CpuTimers.pp \
./DSP2833x_GlobalVariableDefs.pp \
./DSP2833x_Mcbsp.pp \
./DSP2833x_PieCtrl.pp \
./DSP2833x_Spi.pp \
./DSP2833x_SysCtrl.pp \
./Lab6F28335Projectcfg_c.pp \
./RTDX_functions.pp \
./coecsl.pp \
./i2c.pp \
./lcd.pp \
./smallprintf.pp \
./user_Lab6F28335Project.pp \
./user_PIFuncs.pp 

GEN_CMDS += \
./Lab6F28335Projectcfg.cmd 

GEN_FILES += \
./Lab6F28335Projectcfg.cmd \
./Lab6F28335Projectcfg.s?? \
./Lab6F28335Projectcfg_c.c 

GEN_HDRS__QUOTED += \
"Lab6F28335Projectcfg.h" \
"Lab6F28335Projectcfg.h??" 

GEN_MISC_FILES__QUOTED += \
"Lab6F28335Project.cdb" 

GEN_FILES__QUOTED += \
"Lab6F28335Projectcfg.cmd" \
"Lab6F28335Projectcfg.s??" \
"Lab6F28335Projectcfg_c.c" 

C_DEPS__QUOTED += \
"28335_dma.pp" \
"28335_ecan.pp" \
"28335_eqep.pp" \
"28335_inits.pp" \
"28335_mcbsp.pp" \
"28335_pwm.pp" \
"28335_serial.pp" \
"28335_spi.pp" \
"DSP2833x_Adc.pp" \
"DSP2833x_CpuTimers.pp" \
"DSP2833x_GlobalVariableDefs.pp" \
"DSP2833x_Mcbsp.pp" \
"DSP2833x_PieCtrl.pp" \
"DSP2833x_Spi.pp" \
"DSP2833x_SysCtrl.pp" \
"Lab6F28335Projectcfg_c.pp" \
"RTDX_functions.pp" \
"coecsl.pp" \
"i2c.pp" \
"lcd.pp" \
"smallprintf.pp" \
"user_Lab6F28335Project.pp" \
"user_PIFuncs.pp" 

S??_DEPS__QUOTED += \
"Lab6F28335Projectcfg.pp" 

OBJS__QUOTED += \
"28335_dma.obj" \
"28335_ecan.obj" \
"28335_eqep.obj" \
"28335_inits.obj" \
"28335_mcbsp.obj" \
"28335_pwm.obj" \
"28335_serial.obj" \
"28335_spi.obj" \
"DSP2833x_ADC_cal.obj" \
"DSP2833x_Adc.obj" \
"DSP2833x_CSMPasswords.obj" \
"DSP2833x_CodeStartBranch.obj" \
"DSP2833x_CpuTimers.obj" \
"DSP2833x_GlobalVariableDefs.obj" \
"DSP2833x_Mcbsp.obj" \
"DSP2833x_PieCtrl.obj" \
"DSP2833x_Spi.obj" \
"DSP2833x_SysCtrl.obj" \
"DSP2833x_usDelay.obj" \
"Lab6F28335Projectcfg.obj" \
"Lab6F28335Projectcfg_c.obj" \
"RTDX_functions.obj" \
"coecsl.obj" \
"i2c.obj" \
"lcd.obj" \
"smallprintf.obj" \
"user_Lab6F28335Project.obj" \
"user_PIFuncs.obj" 

ASM_DEPS__QUOTED += \
"DSP2833x_ADC_cal.pp" \
"DSP2833x_CSMPasswords.pp" \
"DSP2833x_CodeStartBranch.pp" \
"DSP2833x_usDelay.pp" 

C_SRCS__QUOTED += \
"C:/gigous2_bkuo/repo/Lab6F28335Project/src/28335_dma.c" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/src/28335_ecan.c" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/src/28335_eqep.c" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/src/28335_inits.c" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/src/28335_mcbsp.c" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/src/28335_pwm.c" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/src/28335_serial.c" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/src/28335_spi.c" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_common/source/DSP2833x_Adc.c" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_common/source/DSP2833x_CpuTimers.c" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_headers/source/DSP2833x_GlobalVariableDefs.c" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_common/source/DSP2833x_Mcbsp.c" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_common/source/DSP2833x_PieCtrl.c" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_common/source/DSP2833x_Spi.c" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_common/source/DSP2833x_SysCtrl.c" \
"./Lab6F28335Projectcfg_c.c" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/src/RTDX_functions.c" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/src/coecsl.c" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/src/i2c.c" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/src/lcd.c" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/src/smallprintf.c" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/src/user_Lab6F28335Project.c" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/src/user_PIFuncs.c" 

ASM_SRCS__QUOTED += \
"C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_common/source/DSP2833x_ADC_cal.asm" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_common/source/DSP2833x_CSMPasswords.asm" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_common/source/DSP2833x_CodeStartBranch.asm" \
"C:/gigous2_bkuo/repo/Lab6F28335Project/v110/DSP2833x_common/source/DSP2833x_usDelay.asm" 

GEN_CMDS__FLAG += \
-l"./Lab6F28335Projectcfg.cmd" 

S??_SRCS__QUOTED += \
"./Lab6F28335Projectcfg.s??" 

S??_OBJS__QUOTED += \
"Lab6F28335Projectcfg.obj" 


