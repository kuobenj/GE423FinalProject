################################################################################
# Automatically-generated file. Do not edit!
################################################################################

SHELL = cmd.exe

CG_TOOL_ROOT := C:/CCStudio_v6.0/ccsv6/tools/compiler/c2000_6.2.7

ORDERED_OBJS += \
$(GEN_CMDS__FLAG) \
"./user_gigous_kuo_lab5.obj" \
"./smallprintf.obj" \
"./lcd.obj" \
"./i2c.obj" \
"./gigous_kuo_lab5cfg_c.obj" \
"./gigous_kuo_lab5cfg.obj" \
"./coecsl.obj" \
"./RTDX_functions.obj" \
"./DSP2833x_usDelay.obj" \
"./DSP2833x_SysCtrl.obj" \
"./DSP2833x_Spi.obj" \
"./DSP2833x_PieCtrl.obj" \
"./DSP2833x_Mcbsp.obj" \
"./DSP2833x_GlobalVariableDefs.obj" \
"./DSP2833x_CpuTimers.obj" \
"./DSP2833x_CodeStartBranch.obj" \
"./DSP2833x_CSMPasswords.obj" \
"./DSP2833x_Adc.obj" \
"./DSP2833x_ADC_cal.obj" \
"./28335_spi.obj" \
"./28335_serial.obj" \
"./28335_pwm.obj" \
"./28335_mcbsp.obj" \
"./28335_inits.obj" \
"./28335_eqep.obj" \
"./28335_ecan.obj" \
"./28335_dma.obj" \
"C:/gigous2_bkuo/repo/gigous_kuo_lab5/user_lnk.cmd" \
-l"libc.a" \

-include ../makefile.init

RM := DEL /F
RMDIR := RMDIR /S/Q

# All of the sources participating in the build are defined here
-include sources.mk
-include subdir_vars.mk
-include subdir_rules.mk
-include objects.mk

ifneq ($(MAKECMDGOALS),clean)
ifneq ($(strip $(S_DEPS)),)
-include $(S_DEPS)
endif
ifneq ($(strip $(S_UPPER_DEPS)),)
-include $(S_UPPER_DEPS)
endif
ifneq ($(strip $(S62_DEPS)),)
-include $(S62_DEPS)
endif
ifneq ($(strip $(C64_DEPS)),)
-include $(C64_DEPS)
endif
ifneq ($(strip $(ASM_DEPS)),)
-include $(ASM_DEPS)
endif
ifneq ($(strip $(CC_DEPS)),)
-include $(CC_DEPS)
endif
ifneq ($(strip $(S55_DEPS)),)
-include $(S55_DEPS)
endif
ifneq ($(strip $(C67_DEPS)),)
-include $(C67_DEPS)
endif
ifneq ($(strip $(C??_DEPS)),)
-include $(C??_DEPS)
endif
ifneq ($(strip $(CLA_DEPS)),)
-include $(CLA_DEPS)
endif
ifneq ($(strip $(CPP_DEPS)),)
-include $(CPP_DEPS)
endif
ifneq ($(strip $(S??_DEPS)),)
-include $(S??_DEPS)
endif
ifneq ($(strip $(C_DEPS)),)
-include $(C_DEPS)
endif
ifneq ($(strip $(C62_DEPS)),)
-include $(C62_DEPS)
endif
ifneq ($(strip $(CXX_DEPS)),)
-include $(CXX_DEPS)
endif
ifneq ($(strip $(C++_DEPS)),)
-include $(C++_DEPS)
endif
ifneq ($(strip $(ASM_UPPER_DEPS)),)
-include $(ASM_UPPER_DEPS)
endif
ifneq ($(strip $(K_DEPS)),)
-include $(K_DEPS)
endif
ifneq ($(strip $(C43_DEPS)),)
-include $(C43_DEPS)
endif
ifneq ($(strip $(S67_DEPS)),)
-include $(S67_DEPS)
endif
ifneq ($(strip $(SA_DEPS)),)
-include $(SA_DEPS)
endif
ifneq ($(strip $(S43_DEPS)),)
-include $(S43_DEPS)
endif
ifneq ($(strip $(OPT_DEPS)),)
-include $(OPT_DEPS)
endif
ifneq ($(strip $(S64_DEPS)),)
-include $(S64_DEPS)
endif
ifneq ($(strip $(C_UPPER_DEPS)),)
-include $(C_UPPER_DEPS)
endif
ifneq ($(strip $(C55_DEPS)),)
-include $(C55_DEPS)
endif
endif

-include ../makefile.defs

# Add inputs and outputs from these tool invocations to the build variables 
EXE_OUTPUTS += \
gigous_kuo_lab5.out \

EXE_OUTPUTS__QUOTED += \
"gigous_kuo_lab5.out" \


# All Target
all: gigous_kuo_lab5.out

# Tool invocations
gigous_kuo_lab5.out: $(OBJS) $(CMD_SRCS) $(GEN_CMDS)
	@echo 'Building target: $@'
	@echo 'Invoking: C2000 Linker'
	"C:/CCStudio_v6.0/ccsv6/tools/compiler/c2000_6.2.7/bin/cl2000" -v28 -ml -mt --float_support=fpu32 -g --define=LARGE_MODEL --define=_DEBUG --define=DSP28_BIOS --define=F28335_CONTROL_CARD --define=F28335_CONTROL_CARD30 --diag_warning=225 --display_error_number -z -m"gigous_kuo_lab5.map" --stack_size=0xc00 --warn_sections -i"C:/CCStudio_v6.0/ccsv6/tools/compiler/c2000_6.2.7/lib" -i"C:/CCStudio_v6.0/ccsv6/tools/compiler/c2000_6.2.7/include" -i"C:/CCStudio_v6.0/bios_5_42_01_09/packages/ti/rtdx/lib/c2000" -i"C:/CCStudio_v6.0/bios_5_42_01_09/packages/ti/bios/lib" --reread_libs --xml_link_info="gigous_kuo_lab5_linkInfo.xml" --rom_model -o "gigous_kuo_lab5.out" $(ORDERED_OBJS)
	@echo 'Finished building target: $@'
	@echo ' '

# Other Targets
clean:
	-$(RM) $(GEN_HDRS__QUOTED)$(GEN_MISC_FILES__QUOTED)$(EXE_OUTPUTS__QUOTED)$(GEN_FILES__QUOTED)$(GEN_CMDS__QUOTED)
	-$(RM) "28335_dma.pp" "28335_ecan.pp" "28335_eqep.pp" "28335_inits.pp" "28335_mcbsp.pp" "28335_pwm.pp" "28335_serial.pp" "28335_spi.pp" "DSP2833x_Adc.pp" "DSP2833x_CpuTimers.pp" "DSP2833x_GlobalVariableDefs.pp" "DSP2833x_Mcbsp.pp" "DSP2833x_PieCtrl.pp" "DSP2833x_Spi.pp" "DSP2833x_SysCtrl.pp" "RTDX_functions.pp" "coecsl.pp" "gigous_kuo_lab5cfg_c.pp" "i2c.pp" "lcd.pp" "smallprintf.pp" "user_gigous_kuo_lab5.pp" 
	-$(RM) "gigous_kuo_lab5cfg.pp" 
	-$(RM) "28335_dma.obj" "28335_ecan.obj" "28335_eqep.obj" "28335_inits.obj" "28335_mcbsp.obj" "28335_pwm.obj" "28335_serial.obj" "28335_spi.obj" "DSP2833x_ADC_cal.obj" "DSP2833x_Adc.obj" "DSP2833x_CSMPasswords.obj" "DSP2833x_CodeStartBranch.obj" "DSP2833x_CpuTimers.obj" "DSP2833x_GlobalVariableDefs.obj" "DSP2833x_Mcbsp.obj" "DSP2833x_PieCtrl.obj" "DSP2833x_Spi.obj" "DSP2833x_SysCtrl.obj" "DSP2833x_usDelay.obj" "RTDX_functions.obj" "coecsl.obj" "gigous_kuo_lab5cfg.obj" "gigous_kuo_lab5cfg_c.obj" "i2c.obj" "lcd.obj" "smallprintf.obj" "user_gigous_kuo_lab5.obj" 
	-$(RM) "DSP2833x_ADC_cal.pp" "DSP2833x_CSMPasswords.pp" "DSP2833x_CodeStartBranch.pp" "DSP2833x_usDelay.pp" 
	-@echo 'Finished clean'
	-@echo ' '

.PHONY: all clean dependents
.SECONDARY:

-include ../makefile.targets

