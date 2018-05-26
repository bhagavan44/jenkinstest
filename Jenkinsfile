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
                def status = powershell(returnStdout: true, script: 'build.ps1 --target=Build')
                println status
            }
        }
    }
}