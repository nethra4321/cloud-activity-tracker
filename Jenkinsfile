pipeline {
    agent any

    environment {
        DOCKERHUB_CREDENTIALS = credentials('dockerhub-credentials') // Jenkins credential ID
        AWS_CREDENTIALS = credentials('ec2-ssh') // Jenkins credential ID
        IMAGE_NAME = 'nethra4321/activity-backend'
        IMAGE_TAG = "latest"
        EC2_USER = 'ubuntu'
        EC2_HOST = '51.20.140.225'
    }

    stage('Deploy on EC2') {
        steps {
            sshagent(credentials: ['ec2-ssh']) {
                sh """
                ssh -o StrictHostKeyChecking=no $EC2_USER@$EC2_HOST '
                    export IMAGE_TAG=$IMAGE_TAG &&

                    docker-compose pull &&
                    docker-compose down &&
                    docker-compose up -d &&

                    docker exec -i redpanda rpk topic create activity-events || true &&

                    docker-compose restart activity-backend
                '
                """
            }
        }
    }

    post {
        success {
            echo "Deployment successful."
        }
        failure {
            echo "Deployment failed."
        }
    }
}
