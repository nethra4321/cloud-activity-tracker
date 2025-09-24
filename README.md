# cloud-activity-tracker
This project tracks user focus on browser tabs using a Vue.js frontend and alerts the .NET backend when attention is lost. Useful for e-learning, productivity tools, or monitoring engagement.

### How to run
### 1. Start Docker Services
From the project root:

```bash
docker compose up
```

### 2. Run the frontend (vue.js)
```bash
cd frontend
npm install (if running for first time)
npm run dev
```

### 3. Run the backend (.NET)
```bash
cd backend
dotnet run
```
