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

        stage('test'){
            when{
                expression {
                    return env.BRANCH_NAME == 'master' || env.BRANCH_NAME == 'develop';
                }
            }
            steps{
                powershell '''
                  ."$PWD/build.ps1" --Target=Test --Configuration=Release
                  '''
                mstest testResultsFile:"**/*.trx", keepLongStdio: true
            }
        }
    }
}