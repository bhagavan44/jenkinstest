pipeline{
    agent any
    tim
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
                powershell returnStdout: true, script: '.\build.ps1 --target=Build'
            }
        }
    }
}