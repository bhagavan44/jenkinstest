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
                powershell(returnStdout: true, script: '${WORKSPACE}/build.ps1 --target=Build')
            }
        }
    }
}