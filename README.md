# 🐞 Bug Ticketing System API

The **Bug Ticketing System** is a powerful web-based RESTful API designed to help software teams report, track, and manage bugs effectively. It supports full user management (Manager, Developer, Tester), project organization, bug tracking, user assignments, and file attachments.

---

## 📌 Base URL


---

## 📘 Table of Contents

- [Features](#-features)
- [User Management](#-user-management)
- [Project Management](#-project-management)
- [Bug Management](#-bug-management)
- [User-Bug Relationships](#-user-bug-relationships)
- [Attachments](#-attachments)
- [Authorization](#-authorization)
- [Localization](#-localization)
- [Developer](#-developer)

---

## 🚀 Features

- 👥 Register/Login with Role (Manager, Developer, Tester)
- 🗃️ Create and manage projects
- 🐛 Create, view, and manage bugs
- 👨‍💻 Assign or unassign users to bugs
- 📎 Upload and manage bug attachments
- 🌍 Arabic and English language support

---

## 👤 User Management

### 🔐 Register a New User


**Headers:**
Accept-Language: ar
**Body:**
```json
{
  "name": "Abdoo",
  "email": "abdo11@gmail.com",
  "password": "Abdo@123",
  "role": "Manager"
}
🔑 Login
POST /users/login
Body:

json
Copy code

{
  "userName": "Abdoo",
  "password": "Abdo@123"
}
✅ Returns a JWT token required for authorized requests.


📂 Project Management
📋 Get All Projects
GET /projects
Get Project by ID
GET /projects/{projectId}
Example:
GET /projects/f8449908-8572-45cd-8c52-99ad34fec813

➕ Create New Project
POST /projects
Headers:
Content-Type: application/json
Accept-Language: ar
Authorization: Bearer {your_token}
Body:
{
  "name": "EF project",
  "description": "EF project Final"
}
🐛 Bug Management
📋 Get All Bugs
GET /bugs
🔍 Get Bug by ID
GET /bugs/{bugId}
Example:
GET /bugs/ee3974f0-3c40-4a0a-a640-f1c2f1e8e282
➕ Create a New Bug
POST /bugs
Headers:
Content-Type: application/json
Accept-Language: ar
Authorization: Bearer {your_token}
Body:
{
  "title": "Migration Crash",
  "description": "The Migration crashes when seeding data.",
  "status": "Open", 
  "projectId": "b9e062ff-3be5-4b4f-a498-57bb8969c89e"
}
👥 User-Bug Relationships
👨‍💼 Assign User to a Bug
POST /bugs/{bugId}/assignees?bugId={bugId}&userId={userId}
POST /bugs/5EB6951B-F203-4285-523C-08DD81AB83E1/assignees?bugId=E1BE76D2-1DA9-489C-28D8-08DD81AC7C73&userId=1D03EC7D-05E7-4731-92DF-08DD813277D6
❌ Remove User from Bug
DELETE /bugs/{bugId}/assignees/{userId}
DELETE /bugs/E1BE76D2-1DA9-489C-28D8-08DD81AC7C73/assignees/1D03EC7D-05E7-4731-92DF-08DD813277D6
📎 Attachments
📂 Get Attachments for a Bug
GET /bugs/{bugId}/attachments
⬆️ Upload Attachment
POST /bugs/{bugId}/attachments
Headers:
Authorization: Bearer {your_token}
Content-Type: multipart/form-data
🗑️ Delete Attachment
DELETE /bugs/{bugId}/attachments/{attachmentId}
Example:
DELETE /bugs/E1A85FD7-8626-4C71-861C-77AE6265EABB/attachments/DDBE1EAF-27F5-4E45-9689-E259B666A2F6
🌍 Localization
The API supports language localization through headers.

Example:
Accept-Language: ar   (for Arabic)
Accept-Language: en   (default for English)
🧑‍💻 Developer
 by Mostafa Bolbol Ramadan

Full Stack .NET Developer | ITI Trainee
