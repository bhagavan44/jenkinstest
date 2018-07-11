pipeline{
    agent any
    options {
        skipDefaultCheckout true
    }
    triggers { 
        pollSCM('H */4 * * 1-5') 
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
            when{
                expression {
                    return env.BRANCH_NAME != 'master' && env.BRANCH_NAME != 'develop';
                }
            }
            steps{
                callPowerShell('--Target=Build --Configuration=Release')
            }
        }

        stage('build and test'){
            when{
                expression {
                    return env.BRANCH_NAME == 'master' || env.BRANCH_NAME == 'develop';
                }
            }
            steps{
                withSonarQubeEnv('SonarCloud') {
                    withCredentials([string(credentialsId: 'sonarId', variable: 'SonarKey')]) {
                        callPowerShell('--Target=Sonar --Configuration=Release --branch=$env:BRANCH_NAME --sonarKey=$env:SonarKey')
                        step([$class: 'MSTestPublisher', testResultsFile:"**/*.trx", failOnError: true, keepLongStdio: true])
                    }
                }
            }
            post {
                success {
                    publishHTML target: [
                        allowMissing: false,
                        alwaysLinkToLastBuild: false,
                        keepAll: true,
                        reportDir: 'coverage',
                        reportFiles: 'index.htm',            
                        reportName: 'Code Coverage Report'
                    ]
                    deleteDir()
                }
            }
        }

        stage('Quality Gate'){
            steps{
                timeout(5) {
                    waitForQualityGate abortPipeline: true
                }
            }
        }
    }
}

def callPowerShell(String arguments){
    powershell $'"$PWD/build.ps1" ${arguments}'
}