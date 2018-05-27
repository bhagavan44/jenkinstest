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
                  powershell '''
                  ."$PWD/build.ps1" --Target=Build --Configuration=Release
                  '''
            }
        }

        stage('buil and test'){
            when{
                expression {
                    return env.BRANCH_NAME == 'master' || env.BRANCH_NAME == 'develop';
                }
            }
            steps{
                withCredentials([string(credentialsId: 'sonarId', variable: 'SonarKey')]) {
                    powershell '''
                    ."$PWD/build.ps1" --Target=Sonar --Configuration=Release --buildNumber=$env:BUILD_NUMBER --branch=$env:BRANCH_NAME --sonarKey=$SonarKey"
                    '''
                    step([$class: 'MSTestPublisher', testResultsFile:"**/*.trx", failOnError: true, keepLongStdio: true])
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
    }
}