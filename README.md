# 🛒 Selling Microservices Project

Bu proje, mikroservis mimarisi ile geliştirilmiş basit bir **satış (selling)** sistemidir. Projede ürünler listelenebilir, sipariş verilebilir ve ödeme işlemleri yapılabilir. Dağıtık sistem altyapısında popüler araçlar ve yaklaşımlar entegre edilmiştir: **CQRS**, **MediatR**, **RabbitMQ**, **Redis**, **Consul**, **Ocelot** gibi teknolojiler kullanılmıştır.

---

## 🧱 Mimaride Kullanılan Teknolojiler

| Teknoloji     | Açıklama |
|--------------|----------|
| **.NET 8**    | Backend servislere güç veren platform |
| **CQRS**      | Command ve Query işlemlerinin ayrılması |
| **MediatR**   | Katmanlar arası loosely coupled iletişim |
| **Ocelot**    | API Gateway yönetimi |
| **Consul**    | Servis keşfi (Service Discovery) ve load balancing |
| **RabbitMQ**  | Mesajlaşma altyapısı, Event-driven mimari |
| **Redis**     | Cacheleme altyapısı |
| **Swagger**   | API dokümantasyonu ve test aracı |

---

## 📦 Servisler

### 🧾 Product Service
- Ürünleri listeleme
- Ürün detayı alma
- Ürün ekleme/güncelleme/silme

### 🛒 Order Service
- Sipariş oluşturma (CQRS ile komut/okuma ayrımı)
- Sipariş listeleme
- Sipariş detayları

### 💳 Payment Service
- Ödeme alma
- Ödeme onay/red işlemleri

### 📦 API Gateway (Ocelot)
- Servisler arası yönlendirme
- Token kontrolü, routing, logging

### 🗂 Event Bus
- RabbitMQ üzerinden event-driven yapının yönetimi
- Örnek event: `OrderCreatedEvent`, `PaymentSucceededEvent`

---

## 🧭 Servis Keşfi

- **Consul** kullanılarak her servis kendi kendini kayıt eder ve Ocelot üzerinden ulaşılabilir olur.
- Load balancing işlemi Consul üzerinden yapılabilir.
