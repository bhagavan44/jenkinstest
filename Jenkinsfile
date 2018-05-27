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
                powershell '''
                  ."$PWD/build.ps1" --Target=Test --Configuration=Release
                  '''
                step([$class: 'MSTestPublisher', testResultsFile:"**/*.trx", failOnError: true, keepLongStdio: true])                
            }
            post {
                success {
                    publishHTML target: [
                        allowMissing: false,
                        alwaysLinkToLastBuild: false,
                        keepAll: true,
                        reportDir: 'coverage',
                        reportFiles: 'index.html',            
                        reportName: 'Code Coverage Report'
                    ]
                }
            }
        }        
    }
}