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
                println "$WORKSPACE"
                //powershell(returnStdout: true, script: "${WORKSPACE}")
            }
        }
    }
}