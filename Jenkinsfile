pipeline{
    agent any
    stages{

        stage('clean'){
            steps{
                deleteDir()
            }
        }

        stage('checkout'){
            steps{
                checkout scm
            }
        }

         stage('build'){
            steps{
                //bat "powershell.exe -NonInteractive -ExecutionPolicy Bypass -Command \"& '.\\build.ps1'\""

                PowerShell(". '.\\build.ps1 --Target=Build --Configuration=Debug'") 
            }
        }
    }
}

def PowerShell(psCmd) {
    psCmd=psCmd.replaceAll("%", "%%")
    bat "powershell.exe -NonInteractive -ExecutionPolicy Bypass -Command \"\$ErrorActionPreference='Stop';[Console]::OutputEncoding=[System.Text.Encoding]::UTF8;$psCmd;EXIT \$global:LastExitCode\""
}