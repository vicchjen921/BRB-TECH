# BRB-TECH
This project contains code for Personal Finance Tracker project. 

### Prerequisites
The project requires Docker running with stock (default) settings.

### Installation
Run docker-compose script:
```
docker-compose up --build
```

Services are up and running as follows;

| Service      | URL / Port              
| ----------- | ----------------------- | 
| ASP.NET API | `localhost:5016`        | 
| SQL Server  | `localhost:1433`        |
| Redis       | `localhost:6379`        |

Web API docs is available at http://localhost:5016/swagger/index.html
