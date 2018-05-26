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
            powershell returnStdout: true, script: 'build.ps1 --target=Build'
        }
    }
}