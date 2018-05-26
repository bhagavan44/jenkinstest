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
                def scriptPath = "$WORKSPACE\\build.ps1"
                //powershell(returnStdout: true, script: "$scriptPath")
                bat "powershell.exe -NonInteractive -ExecutionPolicy Bypass -Command \"& '.\\build.ps1'\""
            }
        }
    }
}