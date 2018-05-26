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
                  powershell '''
                  ."$PWD/build.ps1" --Target=Build --Configuration=Release
                  '''
            }
        }
    }
}