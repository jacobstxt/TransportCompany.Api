using Microsoft.EntityFrameworkCore;
using TransportCompany.Api.Entities;
using TransportCompany.Api.Entities.Enums;

namespace TransportCompany.Api.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(TransportDbContext db, CancellationToken ct = default)
        {
            if (await db.Clients.AnyAsync(ct))
                return;

            var now = DateTime.UtcNow;
            var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
            var currentHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0, DateTimeKind.Utc);

            var agroZakhid = new Client
            {
                FullName = "ТОВ \"Агро-Захід\"",
                Phone = "+380322456789",
                Email = "logistics@agro-zakhid.ua",
                CreatedAt = today.AddDays(-40)
            };
            var budMarket = new Client
            {
                FullName = "ПП \"БудМаркет\"",
                Phone = "+380671234567",
                Email = "orders@budmarket.ua",
                CreatedAt = today.AddDays(-30)
            };
            var kovalchuk = new Client
            {
                FullName = "Ковальчук Марія Іванівна",
                Phone = "+380501112233",
                Email = "m.kovalchuk@gmail.com",
                CreatedAt = today.AddDays(-20)
            };
            var techImport = new Client
            {
                FullName = "ТОВ \"ТехІмпорт\"",
                Phone = "+380442223344",
                Email = null,
                CreatedAt = today.AddDays(-5)
            };

            var man = new Vehicle
            {
                PlateNumber = "BC1234AA", Brand = "MAN", Model = "TGX 18.510",
                CapacityKg = 18000, Status = VehicleStatus.Available
            };
            var actros = new Vehicle
            {
                PlateNumber = "BC5678BH", Brand = "Mercedes-Benz", Model = "Actros 1845",
                CapacityKg = 20000, Status = VehicleStatus.OnTrip
            };
            var volvo = new Vehicle
            {
                PlateNumber = "AA4321KX", Brand = "Volvo", Model = "FH 500",
                CapacityKg = 24000, Status = VehicleStatus.Available
            };
            var renault = new Vehicle
            {
                PlateNumber = "BC9012CE", Brand = "Renault", Model = "Master",
                CapacityKg = 1500, Status = VehicleStatus.Available
            };
            var daf = new Vehicle
            {
                PlateNumber = "BC3456AI", Brand = "DAF", Model = "XF 480",
                CapacityKg = 22000, Status = VehicleStatus.Maintenance
            };

            var ivan = new Driver
            {
                FirstName = "Іван", LastName = "Коваленко", LicenseNumber = "ВХА123456",
                Phone = "+380671000001", HiredAt = new DateTime(2019, 3, 1, 0, 0, 0, DateTimeKind.Utc)
            };
            var petro = new Driver
            {
                FirstName = "Петро", LastName = "Мельник", LicenseNumber = "ВХВ654321",
                Phone = "+380671000002", HiredAt = new DateTime(2021, 6, 15, 0, 0, 0, DateTimeKind.Utc)
            };
            var olena = new Driver
            {
                FirstName = "Олена", LastName = "Шевчук", LicenseNumber = "ВХС112233",
                Phone = "+380671000003", HiredAt = new DateTime(2023, 9, 1, 0, 0, 0, DateTimeKind.Utc)
            };
            var andriy = new Driver
            {
                FirstName = "Андрій", LastName = "Бондар", LicenseNumber = "ВХЕ445566",
                Phone = "+380671000004", HiredAt = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc)
            };

            var permits = new List<DriverVehicle>
            {
                new() { Driver = ivan,   Vehicle = man },
                new() { Driver = ivan,   Vehicle = actros },
                new() { Driver = petro,  Vehicle = actros },
                new() { Driver = petro,  Vehicle = volvo },
                new() { Driver = olena,  Vehicle = renault },
                new() { Driver = andriy, Vehicle = volvo },
                new() { Driver = andriy, Vehicle = daf },
            };

            var o1 = new Order
            {
                Client = agroZakhid,
                PickupAddress = "Львів, вул. Городоцька, 355",
                DeliveryAddress = "Київ, вул. Кирилівська, 104",
                CargoDescription = "Зерно пшениці в біг-бегах",
                CargoWeightKg = 16000, Price = 28000,
                Status = OrderStatus.Delivered,
                CreatedAt = today.AddDays(-12)
            };

            var o2 = new Order
            {
                Client = budMarket,
                PickupAddress = "Львів, вул. Зелена, 147",
                DeliveryAddress = "Одеса, вул. Балківська, 199",
                CargoDescription = "Будівельні суміші на палетах",
                CargoWeightKg = 21000, Price = 39500,
                Status = OrderStatus.Delivered,
                CreatedAt = today.AddDays(-8)
            };

            var o3 = new Order
            {
                Client = agroZakhid,
                PickupAddress = "Львів, вул. Городоцька, 355",
                DeliveryAddress = "Дніпро, просп. Слобожанський, 31",
                CargoDescription = "Добрива мінеральні",
                CargoWeightKg = 18500, Price = 46000,
                Status = OrderStatus.InTransit,
                CreatedAt = today.AddDays(-2)
            };

            var o4 = new Order
            {
                Client = kovalchuk,
                PickupAddress = "Львів, вул. Стрийська, 30",
                DeliveryAddress = "Рівне, вул. Соборна, 112",
                CargoDescription = "Домашні речі (переїзд)",
                CargoWeightKg = 900, Price = 6500,
                Status = OrderStatus.Assigned,
                CreatedAt = today.AddDays(-1)
            };

            var o5 = new Order
            {
                Client = techImport,
                PickupAddress = "Львів, вул. Шевченка, 317",
                DeliveryAddress = "Харків, вул. Академіка Павлова, 160",
                CargoDescription = "Побутова техніка в коробках",
                CargoWeightKg = 7500, Price = 31000,
                Status = OrderStatus.New,
                CreatedAt = today
            };
            var o6 = new Order
            {
                Client = budMarket,
                PickupAddress = "Львів, вул. Зелена, 147",
                DeliveryAddress = "Тернопіль, вул. Бродівська, 44",
                CargoDescription = "Цегла облицювальна",
                CargoWeightKg = 12000, Price = 9800,
                Status = OrderStatus.New,
                CreatedAt = today
            };

            var o7 = new Order
            {
                Client = kovalchuk,
                PickupAddress = "Львів, вул. Наукова, 7",
                DeliveryAddress = "Луцьк, вул. Винниченка, 20",
                CargoDescription = "Меблі",
                CargoWeightKg = 600, Price = 4200,
                Status = OrderStatus.Cancelled,
                CreatedAt = today.AddDays(-15)
            };

            var dep1 = today.AddDays(-11).AddHours(5);
            var t1 = new TripDetails
            {
                Order = o1, Driver = ivan, Vehicle = man,
                PlannedDeparture = dep1, PlannedArrival = dep1.AddHours(9), DistanceKm = 540,
                Logs =
                [
                    new() { ActualTime = dep1.AddMinutes(10), DriverComment = "Виїхав зі складу, вантаж закріплено." },
                    new() { ActualTime = dep1.AddHours(4).AddMinutes(30), DriverComment = "Зупинка на заправці під Житомиром." },
                    new() { ActualTime = dep1.AddHours(9).AddMinutes(25), DriverComment = "Доставлено, накладну підписано." },
                ]
            };

            var dep2 = today.AddDays(-7).AddHours(4);
            var t2 = new TripDetails
            {
                Order = o2, Driver = petro, Vehicle = volvo,
                PlannedDeparture = dep2, PlannedArrival = dep2.AddHours(13), DistanceKm = 790,
                Logs =
                [
                    new() { ActualTime = dep2, DriverComment = "Виїзд за графіком." },
                    new() { ActualTime = dep2.AddHours(7), DriverComment = "Затримка 40 хв: ремонт дороги під Уманню." },
                    new() { ActualTime = dep2.AddHours(13).AddMinutes(40), DriverComment = "Вантаж передано отримувачу." },
                ]
            };

            var dep3 = currentHour.AddHours(-3);
            var t3 = new TripDetails
            {
                Order = o3, Driver = ivan, Vehicle = actros,
                PlannedDeparture = dep3, PlannedArrival = dep3.AddHours(14), DistanceKm = 1010,
                Logs =
                [
                    new() { ActualTime = dep3.AddMinutes(15), DriverComment = "Виїхав, затримка на завантаженні 15 хв." },
                ]
            };

            var dep4 = today.AddDays(1).AddHours(7);
            var t4 = new TripDetails
            {
                Order = o4, Driver = olena, Vehicle = renault,
                PlannedDeparture = dep4, PlannedArrival = dep4.AddHours(3), DistanceKm = 210
            };

            db.Clients.AddRange(agroZakhid, budMarket, kovalchuk, techImport);
            db.Vehicles.AddRange(man, actros, volvo, renault, daf);
            db.Drivers.AddRange(ivan, petro, olena, andriy);
            db.DriverVehicles.AddRange(permits);
            db.Orders.AddRange(o1, o2, o3, o4, o5, o6, o7);
            db.TripDetails.AddRange(t1, t2, t3, t4);

            await db.SaveChangesAsync(ct);
        }
    }
}
