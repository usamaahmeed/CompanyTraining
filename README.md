# 📚 Company Training Platform

A multi-tenant training management system for companies, with support for admin management, course delivery, video-based lessons, exams, and auto-generated certificates. Built with ASP.NET Core and secured using JWT and Identity.

---

## 👥 User Roles

### 🛠 1. Admin Dashboard
- Create and manage **subscription packages** (Name, Price, Duration in days)
- View total **platform revenue** from Stripe-based company subscriptions
- Manage all registered **companies** and their details
- View total number of subscribed companies
- Delete companies (and automatically delete their employees)
- Full admin analytics dashboard

### 🏢 2. Company (Employer)
- Register/Login to the system
- Subscribe to packages (Stripe payment integration)
- Receive an **invoice via email** after successful payment
- Add employees (email + password generation)
- Create **courses**, each with:
  - Modules → Lessons (with video content uploaded via **Cloudinary**)
  - Multiple **quizzes**, either:
    - Generated automatically from question pool
    - Or created manually
- Cannot access platform features unless subscribed

### 👨‍🎓 3. User (Employee)
- Login and access courses created by their company
- Enroll in courses, watch video lessons, and mark them as completed
- Take exams; must pass (≥ 50%) to continue
- When all lessons are completed and all exams are passed:
  - Automatically receives a **PDF certificate** generated via **QuestPDF**

---

## 🛡️ Security & Access Control

- Authentication: **ASP.NET Identity** with **JWT Token**
- Role-based Authorization: Admin / Company / Employee
- Middleware: Enforces access restrictions for companies with **expired subscriptions**
- Endpoint protection for:
  - Unsubscribed companies
  - Expired plan access
  - Unauthorized actions per role

---

## 💰 Subscription & Payments

- Packages created by Admin with:
  - Name, Price, Duration
- Stripe Integration for secure checkout
- After successful payment:
  - Company subscription is activated or extended
  - Invoice is emailed automatically to the company
- Companies blocked automatically via middleware if subscription expires

---

## 📝 Exam Auto Submission (Background Job)

- A background job runs to check if **exam time has ended**
- If so:
  - The user's current answers are **auto-submitted** to the database
- Otherwise:
  - User can manually submit if still within time
- Managed with background services (e.g., HostedService/Hangfire)

---

## 📄 Certificate Generation

- When employee:
  - Completes **all lessons**
  - Passes **all quizzes**
- A PDF Certificate is auto-generated via `QuestPDF`
- Contains: Employee name, Course name, Company, Completion date

---

## 🧠 Technologies Used

| Layer           | Technology             |
|----------------|------------------------|
| Backend        | ASP.NET Core 7, C#     |
| ORM            | Entity Framework Core  |
| Auth           | ASP.NET Identity, JWT  |
| Mapping        | Mapster                |
| PDF Generator  | QuestPDF               |
| Cloud Storage  | Cloudinary (for videos)|
| Payment        | Stripe                 |
| Email          | SMTP or Email Service  |
| API Testing    | Swagger UI             |
| Design Pattern | Repository Pattern     |

---

## 📂 Structure

```
CompanyTraining/
│
├── Areas/
│   ├── Admin/
│   ├── Company/
│   └── User/
│
├── Middleware/
│   └── CompanySubscriptionMiddleware.cs
│
├── Services/
│   └── CertificatePdfGenerator.cs
│
├── Repositories/
│   └── Generic + Custom Interfaces
│
├── Models/
│   └── Course, Lesson, Quiz, Question, Choice, Certificate, etc.
│
├── DTOs/
│   └── Request & Response DTOs
│
└── wwwroot/
```

---

## 🔐 API Highlights

### Auth
```http
POST /api/Auth/Login
POST /api/Auth/Register
```

### Courses (Company)
```http
POST /api/Courses
POST /api/Courses/{id}/Modules
POST /api/Modules/{id}/Lessons
```

### Exam & Quiz
```http
POST /api/Courses/{courseId}/Exams/{quizId}  // Submit exam
```

### Certificate
```http
POST /api/Courses/{courseId}/Certificate
```

### Subscription
```http
PUT /api/Packages/{packageId}/Upgrade
```

---

## 📝 Setup Instructions

1. Clone repo:
```bash
git clone https://github.com/your-username/company-training.git
```

2. Update your DB and Stripe keys in `appsettings.json`

3. Run EF Core migration:
```bash
dotnet ef database update
```

4. Launch the project:
```bash
dotnet run
```

5. Test via Swagger UI:
```
https://localhost:{port}/swagger
```

---

## 📈 Future Enhancements

- Employee progress dashboard
- Notifications & reminders
- Stripe webhook validation
- Admin chart visualizations
- Full background jobs using Hangfire

---

## 👨‍💻 Author

Developed by **Ramy Wael**  
🔗 [LinkedIn](#) • 📧 [your@email.com]

---

## 🪪 License

This project is licensed under the MIT License.