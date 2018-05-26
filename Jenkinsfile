pipeline{
    agent any
    options {
        skipDefaultCheckout true
    }
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
                bat "powershell.exe -NonInteractive -ExecutionPolicy Bypass -Command \"& '.\\build.ps1 --Target=Build --Configuration=Debug'\""
            }
        }
    }
}