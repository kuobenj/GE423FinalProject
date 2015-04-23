Public Class Form1
    Dim DirSelected As Integer

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        FolderBrowserDialog1.ShowDialog()
        Label3.Text = FolderBrowserDialog1.SelectedPath & "\" & TextBox2.Text
        Button2.Enabled = True
        DirSelected = 1
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        If DirSelected = 1 Then
            Label3.Text = FolderBrowserDialog1.SelectedPath & "\" & TextBox2.Text
        End If
    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim retvalue As Object

        Dim ProjectDirectory As String
        Dim ProjectName As String
        Dim Project_project_fileFullPath As Object
        Dim Project_cproject_fileFullPath As Object
        Dim Project_ccsproject_fileFullPath As Object
        Dim Project_postbat_fileFullPath As Object
        Dim Project_projectinclude_fileFullPath As Object
        Dim Project_copybat_fileFullPath As Object

        Dim ProjectSubDirectory As String
        Dim ProjectSub_settings_Dir As String
        Dim ProjectSub_Debug_Dir As String
        Dim Project_DSPBIOS_Dir As String
        Dim Project_include_Dir As String
        Dim Project_src_Dir As String
        Dim Project_v110_Dir As String
        Dim Project_DSP2833x_common_Dir As String
        Dim Project_DSP2833x_common_cmd_Dir As String
        Dim Project_DSP2833x_common_include_Dir As String
        Dim Project_DSP2833x_common_source_Dir As String
        Dim Project_DSP2833x_headers_Dir As String
        Dim Project_DSP2833x_headers_cmd_Dir As String
        Dim Project_DSP2833x_headers_include_Dir As String
        Dim Project_DSP2833x_headers_source_Dir As String


        Dim i As Integer
        Dim Drive As String
        Dim PathStr As String, NameStr As String

        On Error GoTo SaveError

        ProjectName = TextBox2.Text
        Drive = Mid(FolderBrowserDialog1.SelectedPath, 1, 2)
        PathStr = Mid(FolderBrowserDialog1.SelectedPath, 4)
        NameStr = ProjectName

        ' Check for bad characters in path
        For i = 0 To PathStr.Length - 1
            If Not ((Asc(PathStr(i)) > 47 And Asc(PathStr(i)) < 58) _
                    Or (Asc(PathStr(i)) > 64 And Asc(PathStr(i)) < 91) _
                    Or (Asc(PathStr(i)) > 96 And Asc(PathStr(i)) < 123) _
                    Or Asc(PathStr(i)) = 92 Or Asc(PathStr(i)) = 95) Then
                MsgBox("Path contains a non-alphanumeric character or a space." & _
                    Chr(13) & Chr(10) & "Please choose a different path.")

                Exit Sub
            End If
        Next i

        ' Check for bad characters in project name
        For i = 0 To NameStr.Length - 1
            If Not ((Asc(NameStr(i)) > 47 And Asc(NameStr(i)) < 58) _
                    Or (Asc(NameStr(i)) > 64 And Asc(NameStr(i)) < 91) _
                    Or (Asc(NameStr(i)) > 96 And Asc(NameStr(i)) < 123) _
                    Or Asc(NameStr(i)) = 92 Or Asc(NameStr(i)) = 95) Then
                MsgBox("Project name contains a non-alphanumeric character or a space." & _
                    Chr(13) & Chr(10) & "Please choose a different name.")

                Exit Sub
            End If
        Next i


        ProjectDirectory = FolderBrowserDialog1.SelectedPath & "\" & ProjectName & "\"
        ProjectSubDirectory = ProjectDirectory & ProjectName & "Project\"
        ProjectSub_settings_Dir = ProjectSubDirectory & ".settings\"
        ProjectSub_Debug_Dir = ProjectSubDirectory & "Debug\"

        Project_DSPBIOS_Dir = ProjectDirectory & "DSPBIOS\"
        Project_include_Dir = ProjectDirectory & "include\"
        Project_src_Dir = ProjectDirectory & "src\"
        Project_v110_Dir = ProjectDirectory & "v110\"
        Project_DSP2833x_common_Dir = Project_v110_Dir & "DSP2833x_common\"
        Project_DSP2833x_common_cmd_Dir = Project_DSP2833x_common_Dir & "cmd\"
        Project_DSP2833x_common_include_Dir = Project_DSP2833x_common_Dir & "include\"
        Project_DSP2833x_common_source_Dir = Project_DSP2833x_common_Dir & "source\"
        Project_DSP2833x_headers_Dir = Project_v110_Dir & "DSP2833x_headers\"
        Project_DSP2833x_headers_cmd_Dir = Project_DSP2833x_headers_Dir & "cmd\"
        Project_DSP2833x_headers_include_Dir = Project_DSP2833x_headers_Dir & "include\"
        Project_DSP2833x_headers_source_Dir = Project_DSP2833x_headers_Dir & "source\"





        Project_project_fileFullPath = ProjectSubDirectory & ".project"
        Project_cproject_fileFullPath = ProjectSubDirectory & ".cproject"
        Project_ccsproject_fileFullPath = ProjectSubDirectory & ".ccsproject"
        Project_postbat_fileFullPath = ProjectDirectory & "postBuildStep_Debug.bat"
        Project_copybat_fileFullPath = ProjectDirectory & "tmpcopy.bat"
        Project_projectinclude_fileFullPath = Project_include_Dir & "user_includes.h"

        MkDir(ProjectDirectory)
        MkDir(ProjectSubDirectory)
        MkDir(ProjectSub_settings_Dir)
        MkDir(ProjectSub_Debug_Dir)
        MkDir(Project_DSPBIOS_Dir)
        MkDir(Project_include_Dir)
        MkDir(Project_src_Dir)
        MkDir(Project_v110_Dir)
        MkDir(Project_DSP2833x_common_Dir)
        MkDir(Project_DSP2833x_common_cmd_Dir)
        MkDir(Project_DSP2833x_common_include_Dir)
        MkDir(Project_DSP2833x_common_source_Dir)
        MkDir(Project_DSP2833x_headers_Dir)
        MkDir(Project_DSP2833x_headers_cmd_Dir)
        MkDir(Project_DSP2833x_headers_include_Dir)
        MkDir(Project_DSP2833x_headers_source_Dir)


        GoTo NoError
SaveError:
        'MsgBox "Error" & CStr(Err.Number)
        If Err.Number = 75 Then
            MsgBox("Directory already Exists!")
            GoTo NoWrite
        End If
NoError:

        FileOpen(1, Project_project_fileFullPath, OpenMode.Output)

        PrintLine(1, "<?xml version=""1.0"" encoding=""UTF-8""?>")
        PrintLine(1, "<projectDescription>")
        PrintLine(1, "	<name>" & ProjectName & "</name>")
        PrintLine(1, "	<comment></comment>")
        PrintLine(1, "	<projects>")
        PrintLine(1, "	</projects>")
        PrintLine(1, "	<buildSpec>")
        PrintLine(1, "		<buildCommand>")
        PrintLine(1, "			<name>org.eclipse.cdt.managedbuilder.core.genmakebuilder</name>")
        PrintLine(1, "			<arguments>")
        PrintLine(1, "				<dictionary>")
        PrintLine(1, "					<key>?name?</key>")
        PrintLine(1, "					<value></value>")
        PrintLine(1, "				</dictionary>")
        PrintLine(1, "				<dictionary>")
        PrintLine(1, "					<key>org.eclipse.cdt.make.core.append_environment</key>")
        PrintLine(1, "					<value>true</value>")
        PrintLine(1, "				</dictionary>")
        PrintLine(1, "				<dictionary>")
        PrintLine(1, "					<key>org.eclipse.cdt.make.core.autoBuildTarget</key>")
        PrintLine(1, "					<value>all</value>")
        PrintLine(1, "				</dictionary>")
        PrintLine(1, "				<dictionary>")
        PrintLine(1, "					<key>org.eclipse.cdt.make.core.buildArguments</key>")
        PrintLine(1, "					<value>-k</value>")
        PrintLine(1, "				</dictionary>")
        PrintLine(1, "				<dictionary>")
        PrintLine(1, "					<key>org.eclipse.cdt.make.core.buildCommand</key>")
        PrintLine(1, "					<value>${CCS_UTILS_DIR}/bin/gmake</value>")
        PrintLine(1, "				</dictionary>")
        PrintLine(1, "				<dictionary>")
        PrintLine(1, "					<key>org.eclipse.cdt.make.core.buildLocation</key>")
        PrintLine(1, "					<value>${workspace_loc:/" & ProjectName & "/Debug}</value>")
        PrintLine(1, "				</dictionary>")
        PrintLine(1, "				<dictionary>")
        PrintLine(1, "					<key>org.eclipse.cdt.make.core.cleanBuildTarget</key>")
        PrintLine(1, "					<value>clean</value>")
        PrintLine(1, "				</dictionary>")
        PrintLine(1, "				<dictionary>")
        PrintLine(1, "					<key>org.eclipse.cdt.make.core.contents</key>")
        PrintLine(1, "					<value>org.eclipse.cdt.make.core.activeConfigSettings</value>")
        PrintLine(1, "				</dictionary>")
        PrintLine(1, "				<dictionary>")
        PrintLine(1, "					<key>org.eclipse.cdt.make.core.enableAutoBuild</key>")
        PrintLine(1, "					<value>true</value>")
        PrintLine(1, "				</dictionary>")
        PrintLine(1, "				<dictionary>")
        PrintLine(1, "					<key>org.eclipse.cdt.make.core.enableCleanBuild</key>")
        PrintLine(1, "					<value>true</value>")
        PrintLine(1, "				</dictionary>")
        PrintLine(1, "				<dictionary>")
        PrintLine(1, "					<key>org.eclipse.cdt.make.core.enableFullBuild</key>")
        PrintLine(1, "					<value>true</value>")
        PrintLine(1, "				</dictionary>")
        PrintLine(1, "				<dictionary>")
        PrintLine(1, "					<key>org.eclipse.cdt.make.core.fullBuildTarget</key>")
        PrintLine(1, "					<value>all</value>")
        PrintLine(1, "				</dictionary>")
        PrintLine(1, "				<dictionary>")
        PrintLine(1, "					<key>org.eclipse.cdt.make.core.stopOnError</key>")
        PrintLine(1, "					<value>true</value>")
        PrintLine(1, "				</dictionary>")
        PrintLine(1, "				<dictionary>")
        PrintLine(1, "					<key>org.eclipse.cdt.make.core.useDefaultBuildCmd</key>")
        PrintLine(1, "					<value>true</value>")
        PrintLine(1, "				</dictionary>")
        PrintLine(1, "			</arguments>")
        PrintLine(1, "		</buildCommand>")
        PrintLine(1, "		<buildCommand>")
        PrintLine(1, "			<name>org.eclipse.cdt.managedbuilder.core.ScannerConfigBuilder</name>")
        PrintLine(1, "			<triggers>full,incremental,</triggers>")
        PrintLine(1, "			<arguments>")
        PrintLine(1, "			</arguments>")
        PrintLine(1, "		</buildCommand>")
        PrintLine(1, "	</buildSpec>")
        PrintLine(1, "	<natures>")
        PrintLine(1, "		<nature>org.eclipse.rtsc.xdctools.buildDefinitions.XDC.xdcNature</nature>")
        PrintLine(1, "		<nature>com.ti.ccstudio.core.ccsNature</nature>")
        PrintLine(1, "		<nature>org.eclipse.cdt.core.cnature</nature>")
        PrintLine(1, "		<nature>org.eclipse.cdt.managedbuilder.core.managedBuildNature</nature>")
        PrintLine(1, "		<nature>org.eclipse.cdt.core.ccnature</nature>")
        PrintLine(1, "		<nature>org.eclipse.cdt.managedbuilder.core.ScannerConfigNature</nature>")
        PrintLine(1, "	</natures>")
        PrintLine(1, "	<linkedResources>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>28335_dma.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/28335_dma.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>28335_ecan.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/28335_ecan.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>28335_eqep.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/28335_eqep.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>28335_inits.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/28335_inits.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>28335_mcbsp.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/28335_mcbsp.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>28335_pwm.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/28335_pwm.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>28335_serial.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/28335_serial.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>28335_spi.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/28335_spi.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>DSP2833x_ADC_cal.asm</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/v110/DSP2833x_common/source/DSP2833x_ADC_cal.asm</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>DSP2833x_Adc.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/v110/DSP2833x_common/source/DSP2833x_Adc.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>DSP2833x_CSMPasswords.asm</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/v110/DSP2833x_common/source/DSP2833x_CSMPasswords.asm</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>DSP2833x_CodeStartBranch.asm</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/v110/DSP2833x_common/source/DSP2833x_CodeStartBranch.asm</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>DSP2833x_CpuTimers.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/v110/DSP2833x_common/source/DSP2833x_CpuTimers.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>DSP2833x_GlobalVariableDefs.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/v110/DSP2833x_headers/source/DSP2833x_GlobalVariableDefs.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>DSP2833x_Mcbsp.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/v110/DSP2833x_common/source/DSP2833x_Mcbsp.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>DSP2833x_PieCtrl.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/v110/DSP2833x_common/source/DSP2833x_PieCtrl.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>DSP2833x_Spi.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/v110/DSP2833x_common/source/DSP2833x_Spi.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>DSP2833x_SysCtrl.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/v110/DSP2833x_common/source/DSP2833x_SysCtrl.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>DSP2833x_usDelay.asm</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/v110/DSP2833x_common/source/DSP2833x_usDelay.asm</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>RTDX_functions.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/RTDX_functions.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>coecsl.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/coecsl.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>i2c.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/i2c.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>lcd.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/lcd.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>smallprintf.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/smallprintf.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>" & ProjectName & ".tcf</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/DSPBIOS/" & ProjectName & ".tcf</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>user_lnk.cmd</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/user_lnk.cmd</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>user_" & ProjectName & ".c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/user_" & ProjectName & ".c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "	</linkedResources>")
        PrintLine(1, "</projectDescription>")
        PrintLine(1, "")


        FileClose(1)


        FileOpen(1, Project_cproject_fileFullPath, OpenMode.Output)
        PrintLine(1, "<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>")
        PrintLine(1, "<?fileVersion 4.0.0?>")
        PrintLine(1, "")
        PrintLine(1, "<cproject storage_type_id=""org.eclipse.cdt.core.XmlProjectDescriptionStorage"">")
        PrintLine(1, "	<storageModule configRelations=""2"" moduleId=""org.eclipse.cdt.core.settings"">")
        PrintLine(1, "		<cconfiguration id=""com.ti.ccstudio.buildDefinitions.C2000.Debug.395831631"">")
        PrintLine(1, "			<storageModule buildSystemId=""org.eclipse.cdt.managedbuilder.core.configurationDataProvider"" id=""com.ti.ccstudio.buildDefinitions.C2000.Debug.395831631"" moduleId=""org.eclipse.cdt.core.settings"" name=""Debug"">")
        PrintLine(1, "				<externalSettings/>")
        PrintLine(1, "				<extensions>")
        PrintLine(1, "					<extension id=""com.ti.ccstudio.binaryparser.CoffParser"" point=""org.eclipse.cdt.core.BinaryParser""/>")
        PrintLine(1, "					<extension id=""com.ti.ccstudio.errorparser.CoffErrorParser"" point=""org.eclipse.cdt.core.ErrorParser""/>")
        PrintLine(1, "					<extension id=""com.ti.ccstudio.errorparser.LinkErrorParser"" point=""org.eclipse.cdt.core.ErrorParser""/>")
        PrintLine(1, "					<extension id=""com.ti.ccstudio.errorparser.AsmErrorParser"" point=""org.eclipse.cdt.core.ErrorParser""/>")
        PrintLine(1, "				</extensions>")
        PrintLine(1, "			</storageModule>")
        PrintLine(1, "			<storageModule moduleId=""cdtBuildSystem"" version=""4.0.0"">")
        PrintLine(1, "				<configuration artifactExtension=""out"" artifactName=""${ProjName}"" buildProperties="""" cleanCommand=""${CG_CLEAN_CMD}"" description="""" id=""com.ti.ccstudio.buildDefinitions.C2000.Debug.395831631"" name=""Debug"" parent=""com.ti.ccstudio.buildDefinitions.C2000.Debug"">")
        PrintLine(1, "					<folderInfo id=""com.ti.ccstudio.buildDefinitions.C2000.Debug.395831631."" name=""/"" resourcePath="""">")
        PrintLine(1, "						<toolChain id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.exe.DebugToolchain.725218749"" name=""TI Build Tools"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.exe.DebugToolchain"" targetTool=""com.ti.ccstudio.buildDefinitions.C2000_6.2.exe.linkerDebug.952650135"">")
        PrintLine(1, "							<option id=""com.ti.ccstudio.buildDefinitions.core.OPT_TAGS.948127859"" superClass=""com.ti.ccstudio.buildDefinitions.core.OPT_TAGS"" valueType=""stringList"">")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""DEVICE_CONFIGURATION_ID=TMS320C28XX.TMS320F28335""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""DEVICE_ENDIANNESS=little""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""OUTPUT_FORMAT=COFF""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""CCS_MBS_VERSION=5.1.0.01""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""LINKER_COMMAND_FILE=user_lnk.cmd""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""RUNTIME_SUPPORT_LIBRARY=libc.a""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""DSPBIOS_VERSION=5.42.01.09""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""OUTPUT_TYPE=bios5Application:rtscApplication:executable""/>")
        PrintLine(1, "							</option>")
        PrintLine(1, "							<option id=""com.ti.ccstudio.buildDefinitions.core.OPT_CODEGEN_VERSION.215843378"" superClass=""com.ti.ccstudio.buildDefinitions.core.OPT_CODEGEN_VERSION"" value=""6.2.7"" valueType=""string""/>")
        PrintLine(1, "							<targetPlatform id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.exe.targetPlatformDebug.597717943"" name=""Platform"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.exe.targetPlatformDebug""/>")
        PrintLine(1, "							<builder buildPath=""${BuildDirectory}"" id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.exe.builderDebug.319333621"" name=""GNU Make.Debug"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.exe.builderDebug""/>")
        PrintLine(1, "							<tool id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.exe.compilerDebug.1745527551"" name=""C2000 Compiler"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.exe.compilerDebug"">")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compilerID.LARGE_MEMORY_MODEL.422936775"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compilerID.LARGE_MEMORY_MODEL"" value=""true"" valueType=""boolean""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compilerID.UNIFIED_MEMORY.1894742821"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compilerID.UNIFIED_MEMORY"" value=""true"" valueType=""boolean""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compilerID.SILICON_VERSION.402840730"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compilerID.SILICON_VERSION"" value=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compilerID.SILICON_VERSION.28"" valueType=""enumerated""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compilerID.FLOAT_SUPPORT.1440192243"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compilerID.FLOAT_SUPPORT"" value=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compilerID.FLOAT_SUPPORT.fpu32"" valueType=""enumerated""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compilerID.DEBUGGING_MODEL.1492183601"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compilerID.DEBUGGING_MODEL"" value=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compilerID.DEBUGGING_MODEL.SYMDEBUG__DWARF"" valueType=""enumerated""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compilerID.INCLUDE_PATH.943674177"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compilerID.INCLUDE_PATH"" valueType=""includePath"">")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${CG_TOOL_ROOT}/include&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${PROJECT_ROOT}/../../mcbsp_com&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${PROJECT_ROOT}/../include&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${PROJECT_ROOT}/../v110/DSP2833x_headers/include&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${PROJECT_ROOT}/../v110/DSP2833x_common/include&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${TCONF_OUTPUT_DIR}&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${BIOS_INCLUDE_DIR}&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${RTDX_INCLUDE_DIR}&quot;""/>")
        PrintLine(1, "								</option>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compilerID.DEFINE.1435129111"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compilerID.DEFINE"" valueType=""definedSymbols"">")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""LARGE_MODEL""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""_DEBUG""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""DSP28_BIOS""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""F28335_CONTROL_CARD""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""F28335_CONTROL_CARD30""/>")
        PrintLine(1, "								</option>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compilerID.DIAG_WARNING.2146556980"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compilerID.DIAG_WARNING"" valueType=""stringList"">")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""225""/>")
        PrintLine(1, "								</option>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compilerID.DISPLAY_ERROR_NUMBER.1759825354"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compilerID.DISPLAY_ERROR_NUMBER"" value=""true"" valueType=""boolean""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compiler.inputType__C_SRCS.1942425575"" name=""C Sources"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compiler.inputType__C_SRCS""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compiler.inputType__CPP_SRCS.562250180"" name=""C++ Sources"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compiler.inputType__CPP_SRCS""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compiler.inputType__ASM_SRCS.1866855487"" name=""Assembly Sources"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compiler.inputType__ASM_SRCS""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compiler.inputType__ASM2_SRCS.1358137010"" name=""Assembly Sources"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.compiler.inputType__ASM2_SRCS""/>")
        PrintLine(1, "							</tool>")
        PrintLine(1, "							<tool id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.exe.linkerDebug.952650135"" name=""C2000 Linker"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.exe.linkerDebug"">")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.linkerID.STACK_SIZE.302440442"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.linkerID.STACK_SIZE"" value=""0xc00"" valueType=""string""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.linkerID.OUTPUT_FILE.526332480"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.linkerID.OUTPUT_FILE"" value=""&quot;${ProjName}.out&quot;"" valueType=""string""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.linkerID.MAP_FILE.345504732"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.linkerID.MAP_FILE"" value=""&quot;${ProjName}.map&quot;"" valueType=""string""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.linkerID.SEARCH_PATH.236208874"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.linkerID.SEARCH_PATH"" valueType=""stringList"">")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${CG_TOOL_ROOT}/lib&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${CG_TOOL_ROOT}/include&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${RTDX_LIB_DIR}&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${BIOS_LIB_DIR}&quot;""/>")
        PrintLine(1, "								</option>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.linkerID.LIBRARY.184393750"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.linkerID.LIBRARY"" valueType=""libs"">")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;libc.a&quot;""/>")
        PrintLine(1, "								</option>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.exeLinker.inputType__CMD_SRCS.113389310"" name=""Linker Command Files"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.exeLinker.inputType__CMD_SRCS""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.exeLinker.inputType__CMD2_SRCS.1613188343"" name=""Linker Command Files"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.exeLinker.inputType__CMD2_SRCS""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.exeLinker.inputType__GEN_CMDS.480326381"" name=""Generated Linker Command Files"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.exeLinker.inputType__GEN_CMDS""/>")
        PrintLine(1, "							</tool>")
        PrintLine(1, "							<tool id=""com.ti.rtsc.buildDefinitions.DSPBIOS_5.42.tool.1462578911"" name=""TConf"" superClass=""com.ti.rtsc.buildDefinitions.DSPBIOS_5.42.tool"">")
        PrintLine(1, "								<inputType id=""com.ti.rtsc.buildDefinitions.DSPBIOS_5.42.tool.inputType__TCF.1854523726"" name=""TConf Scripts"" superClass=""com.ti.rtsc.buildDefinitions.DSPBIOS_5.42.tool.inputType__TCF""/>")
        PrintLine(1, "							</tool>")
        PrintLine(1, "						</toolChain>")
        PrintLine(1, "					</folderInfo>")
        PrintLine(1, "					<fileInfo id=""com.ti.ccstudio.buildDefinitions.C2000.Debug.395831631.28335_RAM_lnk.cmd"" name=""28335_RAM_lnk.cmd"" rcbsApplicability=""disable"" resourcePath=""28335_RAM_lnk.cmd"" toolsToInvoke=""com.ti.ccstudio.buildDefinitions.C2000_6.2.exe.linkerDebug.952650135.366195639"">")
        PrintLine(1, "						<tool id=""com.ti.ccstudio.buildDefinitions.C2000_6.2.exe.linkerDebug.952650135.366195639"" name=""C2000 Linker"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.2.exe.linkerDebug.952650135""/>")
        PrintLine(1, "					</fileInfo>")
        PrintLine(1, "					<sourceEntries>")
        PrintLine(1, "						<entry excluding=""28335_RAM_lnk.cmd"" flags=""VALUE_WORKSPACE_PATH|RESOLVED"" kind=""sourcePath"" name=""""/>")
        PrintLine(1, "					</sourceEntries>")
        PrintLine(1, "				</configuration>")
        PrintLine(1, "			</storageModule>")
        PrintLine(1, "			<storageModule moduleId=""org.eclipse.cdt.core.externalSettings""/>")
        PrintLine(1, "		</cconfiguration>")
        PrintLine(1, "		<cconfiguration id=""com.ti.ccstudio.buildDefinitions.C2000.Release.1355358651"">")
        PrintLine(1, "			<storageModule buildSystemId=""org.eclipse.cdt.managedbuilder.core.configurationDataProvider"" id=""com.ti.ccstudio.buildDefinitions.C2000.Release.1355358651"" moduleId=""org.eclipse.cdt.core.settings"" name=""Release"">")
        PrintLine(1, "				<externalSettings/>")
        PrintLine(1, "				<extensions>")
        PrintLine(1, "					<extension id=""com.ti.ccstudio.binaryparser.CoffParser"" point=""org.eclipse.cdt.core.BinaryParser""/>")
        PrintLine(1, "					<extension id=""com.ti.ccstudio.errorparser.CoffErrorParser"" point=""org.eclipse.cdt.core.ErrorParser""/>")
        PrintLine(1, "					<extension id=""com.ti.ccstudio.errorparser.LinkErrorParser"" point=""org.eclipse.cdt.core.ErrorParser""/>")
        PrintLine(1, "					<extension id=""com.ti.ccstudio.errorparser.AsmErrorParser"" point=""org.eclipse.cdt.core.ErrorParser""/>")
        PrintLine(1, "				</extensions>")
        PrintLine(1, "			</storageModule>")
        PrintLine(1, "			<storageModule moduleId=""cdtBuildSystem"" version=""4.0.0"">")
        PrintLine(1, "				<configuration artifactExtension=""out"" artifactName=""${ProjName}"" buildProperties="""" cleanCommand=""${CG_CLEAN_CMD}"" description="""" id=""com.ti.ccstudio.buildDefinitions.C2000.Release.1355358651"" name=""Release"" parent=""com.ti.ccstudio.buildDefinitions.C2000.Release"">")
        PrintLine(1, "					<folderInfo id=""com.ti.ccstudio.buildDefinitions.C2000.Release.1355358651."" name=""/"" resourcePath="""">")
        PrintLine(1, "						<toolChain id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.exe.ReleaseToolchain.612289625"" name=""TI Build Tools"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.exe.ReleaseToolchain"" targetTool=""com.ti.ccstudio.buildDefinitions.C2000_6.0.exe.linkerRelease.878204513"">")
        PrintLine(1, "							<option id=""com.ti.ccstudio.buildDefinitions.core.OPT_TAGS.563044137"" superClass=""com.ti.ccstudio.buildDefinitions.core.OPT_TAGS"" valueType=""stringList"">")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""DEVICE_CONFIGURATION_ID=TMS320C28XX.TMS320F28335""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""DEVICE_ENDIANNESS=little""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""OUTPUT_FORMAT=COFF""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""CCS_MBS_VERSION=5.1.0.01""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""LINKER_COMMAND_FILE=28335_RAM_lnk.cmd""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""RUNTIME_SUPPORT_LIBRARY=libc.a""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""DSPBIOS_VERSION=5.42.01.09""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""OUTPUT_TYPE=executable""/>")
        PrintLine(1, "							</option>")
        PrintLine(1, "							<option id=""com.ti.ccstudio.buildDefinitions.core.OPT_CODEGEN_VERSION.1045522310"" superClass=""com.ti.ccstudio.buildDefinitions.core.OPT_CODEGEN_VERSION"" value=""6.0.2"" valueType=""string""/>")
        PrintLine(1, "							<targetPlatform id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.exe.targetPlatformRelease.1445785202"" name=""Platform"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.exe.targetPlatformRelease""/>")
        PrintLine(1, "							<builder buildPath=""${BuildDirectory}"" id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.exe.builderRelease.1424430254"" name=""GNU Make.Release"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.exe.builderRelease""/>")
        PrintLine(1, "							<tool id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.exe.compilerRelease.2083235078"" name=""C2000 Compiler"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.exe.compilerRelease"">")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compilerID.LARGE_MEMORY_MODEL.429213972"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compilerID.LARGE_MEMORY_MODEL"" value=""true"" valueType=""boolean""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compilerID.UNIFIED_MEMORY.1696593256"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compilerID.UNIFIED_MEMORY"" value=""true"" valueType=""boolean""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compilerID.SILICON_VERSION.2084680904"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compilerID.SILICON_VERSION"" value=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compilerID.SILICON_VERSION.28"" valueType=""enumerated""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compilerID.FLOAT_SUPPORT.2020929416"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compilerID.FLOAT_SUPPORT"" value=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compilerID.FLOAT_SUPPORT.fpu32"" valueType=""enumerated""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compilerID.INCLUDE_PATH.1730891477"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compilerID.INCLUDE_PATH"" valueType=""includePath"">")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${CG_TOOL_ROOT}/include&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${TCONF_OUTPUT_DIR}&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${BIOS_INCLUDE_DIR}&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${RTDX_INCLUDE_DIR}&quot;""/>")
        PrintLine(1, "								</option>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compilerID.DIAG_WARNING.1741464561"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compilerID.DIAG_WARNING"" valueType=""stringList"">")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""225""/>")
        PrintLine(1, "								</option>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compilerID.DISPLAY_ERROR_NUMBER.1494072087"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compilerID.DISPLAY_ERROR_NUMBER"" value=""true"" valueType=""boolean""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compiler.inputType__C_SRCS.1389642072"" name=""C Sources"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compiler.inputType__C_SRCS""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compiler.inputType__CPP_SRCS.1811032121"" name=""C++ Sources"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compiler.inputType__CPP_SRCS""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compiler.inputType__ASM_SRCS.225890239"" name=""Assembly Sources"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compiler.inputType__ASM_SRCS""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compiler.inputType__ASM2_SRCS.82591340"" name=""Assembly Sources"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.compiler.inputType__ASM2_SRCS""/>")
        PrintLine(1, "							</tool>")
        PrintLine(1, "							<tool id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.exe.linkerRelease.878204513"" name=""C2000 Linker"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.exe.linkerRelease"">")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.linkerID.STACK_SIZE.750193941"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.linkerID.STACK_SIZE"" value=""0x300"" valueType=""string""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.linkerID.OUTPUT_FILE.478941517"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.linkerID.OUTPUT_FILE"" value=""&quot;${ProjName}.out&quot;"" valueType=""string""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.linkerID.MAP_FILE.1319489730"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.linkerID.MAP_FILE"" value=""&quot;${ProjName}.map&quot;"" valueType=""string""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.linkerID.SEARCH_PATH.1745513435"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.linkerID.SEARCH_PATH"" valueType=""stringList"">")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${CG_TOOL_ROOT}/lib&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${CG_TOOL_ROOT}/include&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${RTDX_LIB_DIR}&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${BIOS_LIB_DIR}&quot;""/>")
        PrintLine(1, "								</option>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.linkerID.LIBRARY.251902973"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.linkerID.LIBRARY"" valueType=""libs"">")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;libc.a&quot;""/>")
        PrintLine(1, "								</option>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.exeLinker.inputType__CMD_SRCS.1758565414"" name=""Linker Command Files"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.exeLinker.inputType__CMD_SRCS""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.exeLinker.inputType__CMD2_SRCS.621810036"" name=""Linker Command Files"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.exeLinker.inputType__CMD2_SRCS""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.exeLinker.inputType__GEN_CMDS.1993481693"" name=""Generated Linker Command Files"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.exeLinker.inputType__GEN_CMDS""/>")
        PrintLine(1, "							</tool>")
        PrintLine(1, "							<tool id=""com.ti.rtsc.buildDefinitions.DSPBIOS_5.42.tool.1245486083"" name=""TConf Script Compiler"" superClass=""com.ti.rtsc.buildDefinitions.DSPBIOS_5.42.tool"">")
        PrintLine(1, "								<inputType id=""com.ti.rtsc.buildDefinitions.DSPBIOS_5.42.tool.inputType__TCF.1910708175"" name=""TConf Scripts"" superClass=""com.ti.rtsc.buildDefinitions.DSPBIOS_5.42.tool.inputType__TCF""/>")
        PrintLine(1, "							</tool>")
        PrintLine(1, "						</toolChain>")
        PrintLine(1, "					</folderInfo>")
        PrintLine(1, "					<fileInfo id=""com.ti.ccstudio.buildDefinitions.C2000.Release.1355358651.user_lnk.cmd"" name=""user_lnk.cmd"" rcbsApplicability=""disable"" resourcePath=""user_lnk.cmd"" toolsToInvoke=""com.ti.ccstudio.buildDefinitions.C2000_6.0.exe.linkerRelease.878204513.733171926"">")
        PrintLine(1, "						<tool id=""com.ti.ccstudio.buildDefinitions.C2000_6.0.exe.linkerRelease.878204513.733171926"" name=""C2000 Linker"" superClass=""com.ti.ccstudio.buildDefinitions.C2000_6.0.exe.linkerRelease.878204513""/>")
        PrintLine(1, "					</fileInfo>")
        PrintLine(1, "					<sourceEntries>")
        PrintLine(1, "						<entry excluding=""user_lnk.cmd"" flags=""VALUE_WORKSPACE_PATH|RESOLVED"" kind=""sourcePath"" name=""""/>")
        PrintLine(1, "					</sourceEntries>")
        PrintLine(1, "				</configuration>")
        PrintLine(1, "			</storageModule>")
        PrintLine(1, "			<storageModule moduleId=""org.eclipse.cdt.core.externalSettings""/>")
        PrintLine(1, "		</cconfiguration>")
        PrintLine(1, "	</storageModule>")
        PrintLine(1, "	<storageModule moduleId=""cdtBuildSystem"" version=""4.0.0"">")
        PrintLine(1, "		<project id=""" & ProjectName & ".com.ti.ccstudio.buildDefinitions.C2000.ProjectType.2022031806"" name=""C2000"" projectType=""com.ti.ccstudio.buildDefinitions.C2000.ProjectType""/>")
        PrintLine(1, "	</storageModule>")
        PrintLine(1, "	<storageModule moduleId=""org.eclipse.cdt.core.language.mapping"">")
        PrintLine(1, "		<project-mappings>")
        PrintLine(1, "			<content-type-mapping configuration="""" content-type=""org.eclipse.cdt.core.asmSource"" language=""com.ti.ccstudio.core.TIASMLanguage""/>")
        PrintLine(1, "			<content-type-mapping configuration="""" content-type=""org.eclipse.cdt.core.cHeader"" language=""com.ti.ccstudio.core.TIGCCLanguage""/>")
        PrintLine(1, "			<content-type-mapping configuration="""" content-type=""org.eclipse.cdt.core.cSource"" language=""com.ti.ccstudio.core.TIGCCLanguage""/>")
        PrintLine(1, "			<content-type-mapping configuration="""" content-type=""org.eclipse.cdt.core.cxxHeader"" language=""com.ti.ccstudio.core.TIGPPLanguage""/>")
        PrintLine(1, "			<content-type-mapping configuration="""" content-type=""org.eclipse.cdt.core.cxxSource"" language=""com.ti.ccstudio.core.TIGPPLanguage""/>")
        PrintLine(1, "		</project-mappings>")
        PrintLine(1, "	</storageModule>")
        PrintLine(1, "	<storageModule moduleId=""refreshScope""/>")
        PrintLine(1, "	<storageModule moduleId=""scannerConfiguration""/>")
        PrintLine(1, "	<storageModule moduleId=""org.eclipse.cdt.core.LanguageSettingsProviders""/>")
        PrintLine(1, "</cproject>")
        PrintLine(1, "")

        FileClose(1)

        FileOpen(1, Project_ccsproject_fileFullPath, OpenMode.Output)

        PrintLine(1, "<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>")
        PrintLine(1, "<?ccsproject version=""1.0""?>")
        PrintLine(1, "")
        PrintLine(1, "<projectOptions>")
        PrintLine(1, "<deviceVariant value=""TMS320C28XX.TMS320F28335""/>")
        PrintLine(1, "<deviceEndianness value=""little""/>")
        PrintLine(1, "<codegenToolVersion value=""6.0.2""/>")
        PrintLine(1, "<isElfFormat value=""false""/>")
        PrintLine(1, "<connection value=""common/targetdb/connections/SD510USB_Connection.xml""/>")
        PrintLine(1, "<rts value=""libc.a""/>")
        PrintLine(1, "<templateProperties value=""id=com.ti.common.project.core.emptyProjectTemplate,""/>")
        PrintLine(1, "<deviceFamily value=""C2000"")/>")
        PrintLine(1, "<isTargetManual value=""false""/>")
        PrintLine(1, "</projectOptions>")
        PrintLine(1, "")

        FileClose(1)


        FileOpen(1, Project_postbat_fileFullPath, OpenMode.Output)

        PrintLine(1, "@echo off")
        PrintLine(1, "pushd ..\..\")
        PrintLine(1, "setlocal")
        PrintLine(1, "")
        PrintLine(1, ":process_arg")
        PrintLine(1, "if ""%1""=="""" goto end_process_arg")
        PrintLine(1, "set name=%1")
        PrintLine(1, "set value=")
        PrintLine(1, "")
        PrintLine(1, ":process_arg_value")
        PrintLine(1, "if NOT ""%value%""=="""" set value=%value% %2")
        PrintLine(1, "if ""%value%""=="""" set value=%2")
        PrintLine(1, "shift")
        PrintLine(1, "if ""%2""==""!"" goto set_arg")
        PrintLine(1, "if ""%2""=="""" goto set_arg")
        PrintLine(1, "goto process_arg_value")
        PrintLine(1, "")
        PrintLine(1, ":set_arg")
        PrintLine(1, "set %name%=%value%")
        PrintLine(1, "shift")
        PrintLine(1, "shift")
        PrintLine(1, "goto process_arg")
        PrintLine(1, ":end_process_arg")
        PrintLine(1, "")
        PrintLine(1, "echo. > temp_postBuildStep_Debug.bat")
        PrintLine(1, "")
        PrintLine(1, "echo hex6x.exe %PROJECT_ROOT%\..\out2bootbin.cmd -o %PROJECT_ROOT%\..\" & ProjectName & ".bin %PROJECT_ROOT%\debug\" & ProjectName & ".out >> temp_postBuildStep_Debug.bat")
        PrintLine(1, "")
        PrintLine(1, "call temp_postBuildStep_Debug.bat")
        PrintLine(1, "del temp_postBuildStep_Debug.bat")
        PrintLine(1, "")
        PrintLine(1, "endlocal")
        PrintLine(1, "popd")
        PrintLine(1, " ")

        FileClose(1)

        FileOpen(1, Project_projectinclude_fileFullPath, OpenMode.Output)

        PrintLine(1, "#include """ & ProjectName & "cfg.h""")
        PrintLine(1, "")
        PrintLine(1, " ")

        FileClose(1)

        FileOpen(1, Project_copybat_fileFullPath, OpenMode.Output)

        PrintLine(1, "copy " & ProjectDirectory & "..\Spr2015F28335ProjectCreator\*.cmd " & ProjectDirectory)
        PrintLine(1, "copy " & ProjectDirectory & "..\Spr2015F28335ProjectCreator\DSPBIOS\PROJECTNAME.tcf " & Project_DSPBIOS_Dir & ProjectName & ".tcf")
        PrintLine(1, "copy " & ProjectDirectory & "..\Spr2015F28335ProjectCreator\include\*.* " & Project_include_Dir)
        PrintLine(1, "copy " & ProjectDirectory & "..\Spr2015F28335ProjectCreator\src\*.* " & Project_src_Dir)
        PrintLine(1, "move " & Project_src_Dir & "user_PROJECTNAME.c " & Project_src_Dir & "user_" & ProjectName & ".c")
        PrintLine(1, "copy " & ProjectDirectory & "..\Spr2015F28335ProjectCreator\project\*.ccxml " & ProjectSubDirectory)
        PrintLine(1, "copy " & ProjectDirectory & "..\Spr2015F28335ProjectCreator\project\.settings\*.* " & ProjectSub_settings_Dir)
        PrintLine(1, "copy " & ProjectDirectory & "..\Spr2015F28335ProjectCreator\v110\DSP2833x_common\cmd\*.* " & Project_DSP2833x_common_cmd_Dir)
        PrintLine(1, "copy " & ProjectDirectory & "..\Spr2015F28335ProjectCreator\v110\DSP2833x_common\include\*.* " & Project_DSP2833x_common_include_Dir)
        PrintLine(1, "copy " & ProjectDirectory & "..\Spr2015F28335ProjectCreator\v110\DSP2833x_common\source\*.* " & Project_DSP2833x_common_source_Dir)
        PrintLine(1, "copy " & ProjectDirectory & "..\Spr2015F28335ProjectCreator\v110\DSP2833x_headers\cmd\*.* " & Project_DSP2833x_headers_cmd_Dir)
        PrintLine(1, "copy " & ProjectDirectory & "..\Spr2015F28335ProjectCreator\v110\DSP2833x_headers\include\*.* " & Project_DSP2833x_headers_include_Dir)
        PrintLine(1, "copy " & ProjectDirectory & "..\Spr2015F28335ProjectCreator\v110\DSP2833x_headers\source\*.* " & Project_DSP2833x_headers_source_Dir)
        PrintLine(1, Drive)
        PrintLine(1, "cd """ & ProjectDirectory & """")
        PrintLine(1, "pause")
        PrintLine(1, "del """ & Project_copybat_fileFullPath & """")

        FileClose(1)


        Shell(Project_copybat_fileFullPath, 1)


        Me.Close()

NoWrite:

    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
