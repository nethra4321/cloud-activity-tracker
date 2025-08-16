pipeline {
    agent any

    environment {
        DOCKERHUB_CREDENTIALS = credentials('dockerhub-credentials') 
        AWS_CREDENTIALS = credentials('ec2-ssh') 
        IMAGE_NAME = 'nethra4321/activity-backend'
        IMAGE_TAG = "latest"
        EC2_USER = 'ubuntu'
        EC2_HOST = '51.20.140.225'
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'master', url: 'https://github.com/nethra4321/cloud-activity-tracker.git'
            }
        }

        stage('Build Docker Image') {
            steps {
                 sh 'docker build -t nethra4321/activity-backend:latest ./backend'
            }
        }

        stage('Push to Docker Hub') {
            steps {
                sh """
                echo "$DOCKERHUB_CREDENTIALS_PSW" | docker login -u "$DOCKERHUB_CREDENTIALS_USR" --password-stdin
                docker push $IMAGE_NAME:$IMAGE_TAG
                """
            }
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

                        docker restart activity-backend
                    '
                    """
                }
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
