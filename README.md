# 🎟️ EventServiceProvider
---
## 🧠 AI-användning

***Denna README är delvis genererad med ChatGPT enligt mina instruktioner gällande innehåll och struktur.***
---
Detta är ett delsystem som ansvarar för att hantera event-data: skapa, uppdatera, hämta och radera event inklusive tillhörande paket och bilduppladdning via Cloudinary. Det är ett fristående backend-API som är publicerat live via Render, och används av frontend-projektet i gruppen.

## 📌 Funktioner

### `GET /api/events`
Hämtar alla events.

### `GET /api/events/{id}`
Hämtar ett specifikt event med detaljer.

### `POST /api/events`
Skapar ett nytt event med valfria paket.

### `PUT /api/events`
Uppdaterar ett existerande event och dess paket.

### `DELETE /api/events/{id}`
Tar bort ett event.

### `POST /api/events/upload-image/{eventId}`
Laddar upp ny bild till ett event (Cloudinary).

### `POST /api/events/increase-tickets`
Ökar antalet sålda biljetter för ett event.

### `GET /api/events/edit/{id}`
Hämtar data för frontend-edit-formulär.

---

## 🔧 Användning (för frontend)

*https://eventserviceprovider.onrender.com*

---

## 📊 Diagram

### Sekvensdiagram – Create Event

![Skärmbild 2025-06-01 211732](https://github.com/user-attachments/assets/7cc39fd8-f3d1-4457-b6fe-c4f7d0d117ea)

### Aktivitetsdiagram – Create Event

![Skärmbild 2025-06-01 212201](https://github.com/user-attachments/assets/9c64e27f-f228-41e0-bd90-f73338886318)
