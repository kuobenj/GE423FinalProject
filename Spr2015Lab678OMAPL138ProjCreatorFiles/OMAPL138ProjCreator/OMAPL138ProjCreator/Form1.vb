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

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DirSelected = 0

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
        Dim Project_PRUcode_Dir As String
        Dim Project_src_Dir As String

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
        Project_PRUcode_Dir = ProjectDirectory & "PRUcode\"
        Project_src_Dir = ProjectDirectory & "src\"


        Project_project_fileFullPath = ProjectSubDirectory & ".project"
        Project_cproject_fileFullPath = ProjectSubDirectory & ".cproject"
        Project_ccsproject_fileFullPath = ProjectSubDirectory & ".ccsproject"
        Project_postbat_fileFullPath = ProjectDirectory & "postBuildStep_Debug.bat"
        Project_copybat_fileFullPath = ProjectDirectory & "tmpcopy.bat"
        Project_projectinclude_fileFullPath = Project_include_Dir & "projectinclude.h"

        MkDir(ProjectDirectory)
        MkDir(ProjectSubDirectory)
        MkDir(ProjectSub_settings_Dir)
        MkDir(ProjectSub_Debug_Dir)
        MkDir(Project_DSPBIOS_Dir)
        MkDir(Project_include_Dir)
        MkDir(Project_PRUcode_Dir)
        MkDir(Project_src_Dir)


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
        PrintLine(1, "      <project>evmomapl138_bsl</project>")
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
        PrintLine(1, "			<name>COECSL_edma3.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/COECSL_edma3.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>COECSL_mcbsp.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/COECSL_mcbsp.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>ColorLCD.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/ColorLCD.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>ColorVision.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/ColorVision.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>LCDprintf.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/LCDprintf.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>Ladar.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/Ladar.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>MatrixMath.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/MatrixMath.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>user_xy.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/user_xy.c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>c674xfastMath.lib</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-2-PROJECT_LOC/c67xmathlib_2_01_00_00/lib/c674xfastMath.lib</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>evmomapl138_bsl.lib</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-2-PROJECT_LOC/bsl/lib/evmomapl138_bsl.lib</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>user_" & ProjectName & ".c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/user_" & ProjectName & ".c</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>" & ProjectName & ".tcf</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/DSPBIOS/" & ProjectName & ".tcf</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>lnk.cmd</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/lnk.cmd</locationURI>")
        PrintLine(1, "		</link>")
        PrintLine(1, "		<link>")
        PrintLine(1, "			<name>pru.c</name>")
        PrintLine(1, "			<type>1</type>")
        PrintLine(1, "			<locationURI>PARENT-1-PROJECT_LOC/src/pru.c</locationURI>")
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
        PrintLine(1, "		<cconfiguration id=""com.ti.ccstudio.buildDefinitions.C6000.Debug.1568655633"">")
        PrintLine(1, "			<storageModule buildSystemId=""org.eclipse.cdt.managedbuilder.core.configurationDataProvider"" id=""com.ti.ccstudio.buildDefinitions.C6000.Debug.1568655633"" moduleId=""org.eclipse.cdt.core.settings"" name=""Debug"">")
        PrintLine(1, "				<externalSettings/>")
        PrintLine(1, "				<extensions>")
        PrintLine(1, "					<extension id=""com.ti.ccstudio.binaryparser.CoffParser"" point=""org.eclipse.cdt.core.BinaryParser""/>")
        PrintLine(1, "					<extension id=""com.ti.ccstudio.errorparser.CoffErrorParser"" point=""org.eclipse.cdt.core.ErrorParser""/>")
        PrintLine(1, "					<extension id=""com.ti.ccstudio.errorparser.LinkErrorParser"" point=""org.eclipse.cdt.core.ErrorParser""/>")
        PrintLine(1, "					<extension id=""com.ti.ccstudio.errorparser.AsmErrorParser"" point=""org.eclipse.cdt.core.ErrorParser""/>")
        PrintLine(1, "				</extensions>")
        PrintLine(1, "			</storageModule>")
        PrintLine(1, "			<storageModule moduleId=""cdtBuildSystem"" version=""4.0.0"">")
        PrintLine(1, "				<configuration artifactExtension=""out"" artifactName=""${ProjName}"" buildProperties="""" cleanCommand=""${CG_CLEAN_CMD}"" description="""" id=""com.ti.ccstudio.buildDefinitions.C6000.Debug.1568655633"" name=""Debug"" parent=""com.ti.ccstudio.buildDefinitions.C6000.Debug"" postbuildStep=""&quot;${PROJECT_ROOT}/../postBuildStep_Debug.bat&quot; PROJECT_ROOT ${PROJECT_ROOT} !"">")
        PrintLine(1, "					<folderInfo id=""com.ti.ccstudio.buildDefinitions.C6000.Debug.1568655633."" name=""/"" resourcePath="""">")
        PrintLine(1, "						<toolChain id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.exe.DebugToolchain.919557007"" name=""TI Build Tools"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.exe.DebugToolchain"" targetTool=""com.ti.ccstudio.buildDefinitions.C6000_7.4.exe.linkerDebug.1488294739"">")
        PrintLine(1, "							<option id=""com.ti.ccstudio.buildDefinitions.core.OPT_TAGS.1770462718"" superClass=""com.ti.ccstudio.buildDefinitions.core.OPT_TAGS"" valueType=""stringList"">")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""DEVICE_CONFIGURATION_ID=com.ti.ccstudio.deviceModel.C6000.GenericC674xDevice""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""DEVICE_ENDIANNESS=little""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""OUTPUT_FORMAT=COFF""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""CCS_MBS_VERSION=5.1.0.01""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""LINKER_COMMAND_FILE=lnk.cmd""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""RUNTIME_SUPPORT_LIBRARY=libc.a""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""DSPBIOS_VERSION=5.42.01.09""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""REFERENCES_ADJUSTED=true""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""LINK_ORDER=""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""OUTPUT_TYPE=bios5Application:rtscApplication:executable""/>")
        PrintLine(1, "							</option>")
        PrintLine(1, "							<option id=""com.ti.ccstudio.buildDefinitions.core.OPT_CODEGEN_VERSION.309112353"" superClass=""com.ti.ccstudio.buildDefinitions.core.OPT_CODEGEN_VERSION"" value=""7.4.8"" valueType=""string""/>")
        PrintLine(1, "							<targetPlatform id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.exe.targetPlatformDebug.566586990"" name=""Platform"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.exe.targetPlatformDebug""/>")
        PrintLine(1, "							<builder buildPath=""${BuildDirectory}"" id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.exe.builderDebug.989021085"" name=""GNU Make.Debug"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.exe.builderDebug""/>")
        PrintLine(1, "							<tool id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.exe.compilerDebug.1548459455"" name=""C6000 Compiler"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.exe.compilerDebug"">")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compilerID.SILICON_VERSION.1291606657"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compilerID.SILICON_VERSION"" value=""6740"" valueType=""string""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compilerID.DEBUGGING_MODEL.956126949"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compilerID.DEBUGGING_MODEL"" value=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compilerID.DEBUGGING_MODEL.SYMDEBUG__DWARF"" valueType=""enumerated""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compilerID.OPT_LEVEL.1231433544"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compilerID.OPT_LEVEL"" value=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compilerID.OPT_LEVEL.3"" valueType=""enumerated""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compilerID.DEFINE.1272167833"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compilerID.DEFINE"" valueType=""definedSymbols"">")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""RUNNING_ON_OMAPL138""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""_DEBUG""/>")
        PrintLine(1, "								</option>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compilerID.INCLUDE_PATH.1267042204"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compilerID.INCLUDE_PATH"" valueType=""includePath"">")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${CG_TOOL_ROOT}/include&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${PROJECT_ROOT}/../../bsl/inc&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${PROJECT_ROOT}/..&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${PROJECT_ROOT}/../../c67xmathlib_2_01_00_00/inc&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${PROJECT_ROOT}/../../mcbsp_com&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${PROJECT_ROOT}/../../sharedmem_com&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${PROJECT_ROOT}/../include&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${TCONF_OUTPUT_DIR}&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${BIOS_INCLUDE_DIR}&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${RTDX_INCLUDE_DIR}&quot;""/>")
        PrintLine(1, "								</option>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compilerID.DISPLAY_ERROR_NUMBER.500697684"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compilerID.DISPLAY_ERROR_NUMBER"" value=""true"" valueType=""boolean""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compilerID.DIAG_WARNING.1493272584"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compilerID.DIAG_WARNING"" valueType=""stringList"">")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""225""/>")
        PrintLine(1, "								</option>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compilerID.ABI.1418729892"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compilerID.ABI"" value=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compilerID.ABI.coffabi"" valueType=""enumerated""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compiler.inputType__C_SRCS.187007849"" name=""C Sources"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compiler.inputType__C_SRCS""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compiler.inputType__CPP_SRCS.854272587"" name=""C++ Sources"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compiler.inputType__CPP_SRCS""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compiler.inputType__ASM_SRCS.1387853780"" name=""Assembly Sources"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compiler.inputType__ASM_SRCS""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compiler.inputType__ASM2_SRCS.207689224"" name=""Assembly Sources"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.compiler.inputType__ASM2_SRCS""/>")
        PrintLine(1, "							</tool>")
        PrintLine(1, "							<tool id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.exe.linkerDebug.1488294739"" name=""C6000 Linker"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.exe.linkerDebug"">")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.linkerID.OUTPUT_FILE.2002725781"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.linkerID.OUTPUT_FILE"" value=""&quot;${ProjName}.out&quot;"" valueType=""string""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.linkerID.MAP_FILE.893386865"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.linkerID.MAP_FILE"" value=""&quot;${ProjName}.map&quot;"" valueType=""string""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.linkerID.STACK_SIZE.1109145708"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.linkerID.STACK_SIZE"" value=""0xc00"" valueType=""string""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.linkerID.SEARCH_PATH.1443050758"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.linkerID.SEARCH_PATH"" valueType=""stringList"">")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${CG_TOOL_ROOT}/lib&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${CG_TOOL_ROOT}/include&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${RTDX_LIB_DIR}&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${BIOS_LIB_DIR}&quot;""/>")
        PrintLine(1, "								</option>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.linkerID.LIBRARY.490270398"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.linkerID.LIBRARY"" valueType=""libs"">")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;libc.a&quot;""/>")
        PrintLine(1, "								</option>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.exeLinker.inputType__CMD_SRCS.1725713409"" name=""Linker Command Files"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.exeLinker.inputType__CMD_SRCS""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.exeLinker.inputType__CMD2_SRCS.758942809"" name=""Linker Command Files"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.exeLinker.inputType__CMD2_SRCS""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C6000_7.4.exeLinker.inputType__GEN_CMDS.1306981975"" name=""Generated Linker Command Files"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.4.exeLinker.inputType__GEN_CMDS""/>")
        PrintLine(1, "							</tool>")
        PrintLine(1, "							<tool id=""com.ti.rtsc.buildDefinitions.DSPBIOS_5.42.tool.1879266043"" name=""TConf"" superClass=""com.ti.rtsc.buildDefinitions.DSPBIOS_5.42.tool"">")
        PrintLine(1, "								<option id=""com.ti.rtsc.buildDefinitions.DSPBIOS_5.42.tool.CONFIG_IMPORT_PATH.2061402392"" superClass=""com.ti.rtsc.buildDefinitions.DSPBIOS_5.42.tool.CONFIG_IMPORT_PATH"" valueType=""includePath"">")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${BIOS_CG_ROOT}/packages&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${PROJECT_ROOT}/../DSPBIOS&quot;""/>")
        PrintLine(1, "								</option>")
        PrintLine(1, "								<inputType id=""com.ti.rtsc.buildDefinitions.DSPBIOS_5.42.tool.inputType__TCF.684104840"" name=""TConf Scripts"" superClass=""com.ti.rtsc.buildDefinitions.DSPBIOS_5.42.tool.inputType__TCF""/>")
        PrintLine(1, "							</tool>")
        PrintLine(1, "						</toolChain>")
        PrintLine(1, "					</folderInfo>")
        PrintLine(1, "				</configuration>")
        PrintLine(1, "			</storageModule>")
        PrintLine(1, "			<storageModule moduleId=""org.eclipse.cdt.core.externalSettings"">")
        PrintLine(1, "				<externalSettings containerId=""evmomapl138_bsl;"" factoryId=""org.eclipse.cdt.core.cfg.export.settings.sipplier""/>")
        PrintLine(1, "			</storageModule>")
        PrintLine(1, "		</cconfiguration>")
        PrintLine(1, "		<cconfiguration id=""com.ti.ccstudio.buildDefinitions.C6000.Release.1617624426"">")
        PrintLine(1, "			<storageModule buildSystemId=""org.eclipse.cdt.managedbuilder.core.configurationDataProvider"" id=""com.ti.ccstudio.buildDefinitions.C6000.Release.1617624426"" moduleId=""org.eclipse.cdt.core.settings"" name=""Release"">")
        PrintLine(1, "				<externalSettings/>")
        PrintLine(1, "				<extensions>")
        PrintLine(1, "					<extension id=""com.ti.ccstudio.binaryparser.CoffParser"" point=""org.eclipse.cdt.core.BinaryParser""/>")
        PrintLine(1, "					<extension id=""com.ti.ccstudio.errorparser.CoffErrorParser"" point=""org.eclipse.cdt.core.ErrorParser""/>")
        PrintLine(1, "					<extension id=""com.ti.ccstudio.errorparser.LinkErrorParser"" point=""org.eclipse.cdt.core.ErrorParser""/>")
        PrintLine(1, "					<extension id=""com.ti.ccstudio.errorparser.AsmErrorParser"" point=""org.eclipse.cdt.core.ErrorParser""/>")
        PrintLine(1, "				</extensions>")
        PrintLine(1, "			</storageModule>")
        PrintLine(1, "			<storageModule moduleId=""cdtBuildSystem"" version=""4.0.0"">")
        PrintLine(1, "				<configuration artifactExtension=""out"" artifactName=""${ProjName}"" buildProperties="""" cleanCommand=""${CG_CLEAN_CMD}"" description="""" id=""com.ti.ccstudio.buildDefinitions.C6000.Release.1617624426"" name=""Release"" parent=""com.ti.ccstudio.buildDefinitions.C6000.Release"">")
        PrintLine(1, "					<folderInfo id=""com.ti.ccstudio.buildDefinitions.C6000.Release.1617624426."" name=""/"" resourcePath="""">")
        PrintLine(1, "						<toolChain id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.exe.ReleaseToolchain.702816853"" name=""TI Build Tools"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.exe.ReleaseToolchain"" targetTool=""com.ti.ccstudio.buildDefinitions.C6000_7.3.exe.linkerRelease.1109741180"">")
        PrintLine(1, "							<option id=""com.ti.ccstudio.buildDefinitions.core.OPT_TAGS.369586644"" superClass=""com.ti.ccstudio.buildDefinitions.core.OPT_TAGS"" valueType=""stringList"">")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""DEVICE_CONFIGURATION_ID=com.ti.ccstudio.deviceModel.C6000.GenericC674xDevice""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""DEVICE_ENDIANNESS=little""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""OUTPUT_FORMAT=COFF""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""CCS_MBS_VERSION=5.1.0.01""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""RUNTIME_SUPPORT_LIBRARY=libc.a""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""DSPBIOS_VERSION=5.42.01.09""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""OUTPUT_TYPE=bios5Application:rtscApplication:executable""/>")
        PrintLine(1, "								<listOptionValue builtIn=""false"" value=""REFERENCES_ADJUSTED=true""/>")
        PrintLine(1, "							</option>")
        PrintLine(1, "							<option id=""com.ti.ccstudio.buildDefinitions.core.OPT_CODEGEN_VERSION.727236130"" name=""Compiler version"" superClass=""com.ti.ccstudio.buildDefinitions.core.OPT_CODEGEN_VERSION"" value=""7.3.1"" valueType=""string""/>")
        PrintLine(1, "							<targetPlatform id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.exe.targetPlatformRelease.2124646405"" name=""Platform"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.exe.targetPlatformRelease""/>")
        PrintLine(1, "							<builder buildPath=""${BuildDirectory}"" id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.exe.builderRelease.1639314349"" keepEnvironmentInBuildfile=""false"" name=""GNU Make"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.exe.builderRelease""/>")
        PrintLine(1, "							<tool id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.exe.compilerRelease.544405131"" name=""C6000 Compiler"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.exe.compilerRelease"">")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.compilerID.SILICON_VERSION.84262898"" name=""Target processor version (--silicon_version, -mv)"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.compilerID.SILICON_VERSION"" value=""6740"" valueType=""string""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.compilerID.DIAG_WARNING.1628561198"" name=""Treat diagnostic &lt;id&gt; as warning (--diag_warning, -pdsw)"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.compilerID.DIAG_WARNING"" valueType=""stringList"">")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""225""/>")
        PrintLine(1, "								</option>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.compilerID.DISPLAY_ERROR_NUMBER.105792120"" name=""Emit diagnostic identifier numbers (--display_error_number, -pden)"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.compilerID.DISPLAY_ERROR_NUMBER"" value=""true"" valueType=""boolean""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.compilerID.INCLUDE_PATH.936319361"" name=""Add dir to #include search path (--include_path, -I)"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.compilerID.INCLUDE_PATH"" valueType=""includePath"">")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${CG_TOOL_ROOT}/include&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${TCONF_OUTPUT_DIR}&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${BIOS_INCLUDE_DIR}&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${RTDX_INCLUDE_DIR}&quot;""/>")
        PrintLine(1, "								</option>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.compilerID.ABI.1937304318"" name=""Application binary interface (coffabi, eabi) (--abi)"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.compilerID.ABI"" value=""com.ti.ccstudio.buildDefinitions.C6000_7.3.compilerID.ABI.coffabi"" valueType=""enumerated""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.compiler.inputType__C_SRCS.587211671"" name=""C Sources"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.compiler.inputType__C_SRCS""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.compiler.inputType__CPP_SRCS.1985567238"" name=""C++ Sources"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.compiler.inputType__CPP_SRCS""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.compiler.inputType__ASM_SRCS.1608574061"" name=""Assembly Sources"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.compiler.inputType__ASM_SRCS""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.compiler.inputType__ASM2_SRCS.1054416242"" name=""Assembly Sources"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.compiler.inputType__ASM2_SRCS""/>")
        PrintLine(1, "							</tool>")
        PrintLine(1, "							<tool id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.exe.linkerRelease.1109741180"" name=""C6000 Linker"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.exe.linkerRelease"">")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.linkerID.OUTPUT_FILE.1710309616"" name=""Specify output file name (--output_file, -o)"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.linkerID.OUTPUT_FILE"" value=""&quot;${ProjName}.out&quot;"" valueType=""string""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.linkerID.MAP_FILE.416743625"" name=""Input and output sections listed into &lt;file&gt; (--map_file, -m)"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.linkerID.MAP_FILE"" value=""&quot;${ProjName}.map&quot;"" valueType=""string""/>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.linkerID.LIBRARY.757085523"" name=""Include library file or command file as input (--library, -l)"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.linkerID.LIBRARY"" valueType=""libs"">")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;libc.a&quot;""/>")
        PrintLine(1, "								</option>")
        PrintLine(1, "								<option id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.linkerID.SEARCH_PATH.1171614045"" name=""Add &lt;dir&gt; to library search path (--search_path, -i)"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.linkerID.SEARCH_PATH"" valueType=""stringList"">")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${CG_TOOL_ROOT}/lib&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${CG_TOOL_ROOT}/include&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${RTDX_LIB_DIR}&quot;""/>")
        PrintLine(1, "									<listOptionValue builtIn=""false"" value=""&quot;${BIOS_LIB_DIR}&quot;""/>")
        PrintLine(1, "								</option>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.exeLinker.inputType__CMD_SRCS.442526966"" name=""Linker Command Files"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.exeLinker.inputType__CMD_SRCS""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.exeLinker.inputType__CMD2_SRCS.1221469514"" name=""Linker Command Files"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.exeLinker.inputType__CMD2_SRCS""/>")
        PrintLine(1, "								<inputType id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.exeLinker.inputType__GEN_CMDS.1978575509"" name=""Generated Linker Command Files"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.exeLinker.inputType__GEN_CMDS""/>")
        PrintLine(1, "							</tool>")
        PrintLine(1, "							<tool id=""com.ti.rtsc.buildDefinitions.DSPBIOS_5.42.tool.1333233446"" name=""TConf Script Compiler"" superClass=""com.ti.rtsc.buildDefinitions.DSPBIOS_5.42.tool"">")
        PrintLine(1, "								<inputType id=""com.ti.rtsc.buildDefinitions.DSPBIOS_5.42.tool.inputType__TCF.989346040"" name=""TConf Scripts"" superClass=""com.ti.rtsc.buildDefinitions.DSPBIOS_5.42.tool.inputType__TCF""/>")
        PrintLine(1, "							</tool>")
        PrintLine(1, "						</toolChain>")
        PrintLine(1, "					</folderInfo>")
        PrintLine(1, "					<fileInfo id=""com.ti.ccstudio.buildDefinitions.C6000.Release.1617624426.lnk.cmd"" name=""lnk.cmd"" rcbsApplicability=""disable"" resourcePath=""lnk.cmd"" toolsToInvoke=""com.ti.ccstudio.buildDefinitions.C6000_7.3.exe.linkerRelease.1109741180.446054551"">")
        PrintLine(1, "						<tool id=""com.ti.ccstudio.buildDefinitions.C6000_7.3.exe.linkerRelease.1109741180.446054551"" name=""C6000 Linker"" superClass=""com.ti.ccstudio.buildDefinitions.C6000_7.3.exe.linkerRelease.1109741180""/>")
        PrintLine(1, "					</fileInfo>")
        PrintLine(1, "					<sourceEntries>")
        PrintLine(1, "						<entry excluding=""lnk.cmd"" flags=""VALUE_WORKSPACE_PATH|RESOLVED"" kind=""sourcePath"" name=""""/>")
        PrintLine(1, "					</sourceEntries>")
        PrintLine(1, "				</configuration>")
        PrintLine(1, "			</storageModule>")
        PrintLine(1, "			<storageModule moduleId=""org.eclipse.cdt.core.externalSettings"">")
        PrintLine(1, "				<externalSettings containerId=""evmomapl138_bsl;"" factoryId=""org.eclipse.cdt.core.cfg.export.settings.sipplier""/>")
        PrintLine(1, "			</storageModule>")
        PrintLine(1, "		</cconfiguration>")
        PrintLine(1, "	</storageModule>")
        PrintLine(1, "	<storageModule moduleId=""cdtBuildSystem"" version=""4.0.0"">")
        PrintLine(1, "		<project id=""" & ProjectName & ".com.ti.ccstudio.buildDefinitions.C6000.ProjectType.390136699"" name=""C6000"" projectType=""com.ti.ccstudio.buildDefinitions.C6000.ProjectType""/>")
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
        PrintLine(1, "<deviceVariant value=""com.ti.ccstudio.deviceModel.C6000.GenericC674xDevice""/>")
        PrintLine(1, "<deviceEndianness value=""little""/>")
        PrintLine(1, "<codegenToolVersion value=""7.3.1""/>")
        PrintLine(1, "<isElfFormat value=""false""/>")
        PrintLine(1, "<connection value=""""/>")
        PrintLine(1, "<rts value=""libc.a""/>")
        PrintLine(1, "<templateProperties value=""id=com.ti.rtsc.DSPBIOS.example_121,type=bios5,products=com.ti.rtsc.DSPBIOS,""/>")
        PrintLine(1, "<deviceFamily value=""C6000""/>")
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

        PrintLine(1, "copy " & ProjectDirectory & "..\Spr2015Lab678OMAPL138ProjCreatorFiles\*.cmd " & ProjectDirectory)
        PrintLine(1, "copy " & ProjectDirectory & "..\Spr2015Lab678OMAPL138ProjCreatorFiles\DSPBIOS\PROJECTNAME.tcf " & Project_DSPBIOS_Dir & ProjectName & ".tcf")
        PrintLine(1, "copy " & ProjectDirectory & "..\Spr2015Lab678OMAPL138ProjCreatorFiles\include\*.* " & Project_include_Dir)
        PrintLine(1, "copy " & ProjectDirectory & "..\Spr2015Lab678OMAPL138ProjCreatorFiles\PRUcode\*.* " & Project_PRUcode_Dir)
        PrintLine(1, "copy " & ProjectDirectory & "..\Spr2015Lab678OMAPL138ProjCreatorFiles\src\*.* " & Project_src_Dir)
        PrintLine(1, "move " & Project_src_Dir & "user_PROJECTNAME.c " & Project_src_Dir & "user_" & ProjectName & ".c")
        PrintLine(1, "copy " & ProjectDirectory & "..\Spr2015Lab678OMAPL138ProjCreatorFiles\project\*.ccxml " & ProjectSubDirectory)
        PrintLine(1, "copy " & ProjectDirectory & "..\Spr2015Lab678OMAPL138ProjCreatorFiles\project\.settings\*.* " & ProjectSub_settings_Dir)
        PrintLine(1, Drive)
        PrintLine(1, "cd """ & ProjectDirectory & """")
        PrintLine(1, "pause")
        PrintLine(1, "del """ & Project_copybat_fileFullPath & """")

        FileClose(1)


        Shell(Project_copybat_fileFullPath, 1)


        Me.Close()

NoWrite:

    End Sub
End Class
