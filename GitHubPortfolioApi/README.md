<<<<<<< HEAD
# CV-Site
=======
# GitHub Portfolio API

אפליקציית ASP.NET Core Web API שמתחברת ל-GitHub ומציגה פורטפוליו של מפתח.

## תכונות

### 1. GetPortfolio - `/api/portfolio`
מחזירה את כל ה-repositories של המשתמש עם:
- שם ה-repository
- תיאור
- קישור (URL)
- **שפות הפיתוח** (עם מספר בתים לכל שפה)
- **תאריך הקומיט האחרון**
- **מספר כוכבים (Stars)**
- **מספר Pull Requests**
- **קישור לאתר (Homepage)** אם קיים

### 2. SearchRepositories - `/api/search`
חיפוש ב-GitHub repositories ציבוריים עם פרמטרים אופציונליים:
- `name` - שם repository
- `language` - שפת פיתוח
- `user` - שם משתמש

דוגמה: `/api/search?language=csharp&user=microsoft`

### 3. Caching חכם
- השימוש ב-**Decorator Pattern** עם `CachedGitHubService`
- בדיקה אוטומטית אם היו עדכונים ב-GitHub לפני שימוש ב-cache
- אם לא היו עדכונים - מחזיר מה-cache (מהיר!)
- אם היו עדכונים - שולף מחדש מ-GitHub

## טכנולוגיות

- **ASP.NET Core 8.0** - Web API
- **Octokit.NET** - חיבור ל-GitHub API
- **In-Memory Caching** - לביצועים מהירים
- **Decorator Pattern** - ארכיטקטורה נקייה
- **User Secrets** - אבטחת טוקן

## התקנה והרצה

### 1. הגדרת User Secrets

הטוקן ושם המשתמש נשמרים ב-User Secrets (לא בקוד!):

```bash
dotnet user-secrets set "GitHub:Username" "l0527626561-coder"
dotnet user-secrets set "GitHub:Token" "YOUR_GITHUB_TOKEN_HERE"
```

### 2. התקנת תלויות

```bash
dotnet restore
```

### 3. הרצת האפליקציה

```bash
dotnet run
```

האפליקציה תרוץ על: `https://localhost:5001` (או HTTP על 5000)

### 4. גישה ל-Swagger

פתח דפדפן וגש ל: `https://localhost:5001/swagger`

## Endpoints

| Method | Path | תיאור |
|--------|------|-------|
| GET | `/api/portfolio` | מחזיר את כל ה-repositories של המשתמש |
| GET | `/api/search?name=X&language=Y&user=Z` | חיפוש repositories ציבוריים |

## מבנה הפרויקט

```
GitHubPortfolioApi/
├── Models/
│   ├── GitHubDtos.cs          # DTOs לrepositories וחיפוש
│   └── GitHubOptions.cs       # Options לטוקן ושם משתמש
├── Services/
│   ├── IGitHubService.cs      # Interface
│   ├── GitHubService.cs       # מימוש עם Octokit
│   └── CachedGitHubService.cs # Decorator עם caching חכם
└── Program.cs                 # הגדרות DI ו-endpoints
```

## דרישות המטלה שמומשו

✅ שימוש ב-**Octokit.NET** במקום HttpClient ישיר  
✅ **Options Pattern** עם User Secrets  
✅ פונקציית **GetPortfolio** עם כל הנתונים הנדרשים  
✅ פונקציית **SearchRepositories** עם פרמטרים אופציונליים  
✅ **In-Memory Caching** עם Decorator Pattern  
✅ **אתגר**: בדיקת עדכונים ב-GitHub לפני שימוש ב-cache  

## הערות

- הטוקן **לעולם לא** נמצא בקוד או ב-appsettings.json
- ב-production יש להגדיר את הטוקן כמשתנה סביבה על השרת
- ה-cache מתרוקן אוטומטית אחרי 30 דקות (גם אם אין עדכונים)
>>>>>>> d24d431 (Initial commit)
