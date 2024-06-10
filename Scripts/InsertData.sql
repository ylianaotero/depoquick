INSERT INTO depoquick.dbo.Users
(IsAdministrator, Name, Email, Password)
VALUES(1, N'Maria Rodriguez', N'mrodriguez@gmail.com', N'Contrasena1#');
INSERT INTO depoquick.dbo.Users
(IsAdministrator, Name, Email, Password)
VALUES(0, N'Juan Perez', N'jperez@gmail.com', N'Contrasena1#');
INSERT INTO depoquick.dbo.Users
(IsAdministrator, Name, Email, Password)
VALUES(0, N'Carlos Vazquez', N'cvazquez@gmail.com', N'Contrasena1#');
INSERT INTO depoquick.dbo.Users
(IsAdministrator, Name, Email, Password)
VALUES(0, N'Clara Sanchez', N'csanchez@gmail.com', N'Contrasena1#');

INSERT INTO depoquick.dbo.Administrators
(Id)
VALUES(1);

INSERT INTO depoquick.dbo.Clients
(Id)
VALUES(2);
INSERT INTO depoquick.dbo.Clients
(Id)
VALUES(3);
INSERT INTO depoquick.dbo.Clients
(Id)
VALUES(4);


INSERT INTO depoquick.dbo.Promotions
(ValidityDate_InitialDate, ValidityDate_FinalDate, Label, DiscountRate)
VALUES('2024-05-27 00:00:00.000', '2024-05-30 00:00:00.000', N'Ola Polar', 0.21);
INSERT INTO depoquick.dbo.Promotions
(ValidityDate_InitialDate, ValidityDate_FinalDate, Label, DiscountRate)
VALUES('2024-06-03 00:00:00.000', '2024-06-07 00:00:00.000', N'Cyber Week', 0.3);
INSERT INTO depoquick.dbo.Promotions
(ValidityDate_InitialDate, ValidityDate_FinalDate, Label, DiscountRate)
VALUES('2024-06-09 18:24:00.401', '2024-06-14 00:00:00.000', N'Liquidación', 0.6);
INSERT INTO depoquick.dbo.Promotions
(ValidityDate_InitialDate, ValidityDate_FinalDate, Label, DiscountRate)
VALUES('2024-06-12 00:00:00.000', '2024-06-13 00:00:00.000', N'Flash Sale', 0.45);
INSERT INTO depoquick.dbo.Promotions
(ValidityDate_InitialDate, ValidityDate_FinalDate, Label, DiscountRate)
VALUES('2024-06-18 00:00:00.000', '2024-06-20 00:00:00.000', N'Gran DepoDescuento', 0.75);
INSERT INTO depoquick.dbo.Promotions
(ValidityDate_InitialDate, ValidityDate_FinalDate, Label, DiscountRate)
VALUES('2024-06-27 00:00:00.000', '2024-06-30 00:00:00.000', N'IVA OFF', 0.21);
INSERT INTO depoquick.dbo.Promotions
(ValidityDate_InitialDate, ValidityDate_FinalDate, Label, DiscountRate)
VALUES('2024-07-05 00:00:00.000', '2024-07-12 00:00:00.000', N'Dia del Padre', 0.1);
INSERT INTO depoquick.dbo.Promotions
(ValidityDate_InitialDate, ValidityDate_FinalDate, Label, DiscountRate)
VALUES('2024-07-11 00:00:00.000', '2024-07-13 00:00:00.000', N'Black Friday', 0.4);
INSERT INTO depoquick.dbo.Promotions
(ValidityDate_InitialDate, ValidityDate_FinalDate, Label, DiscountRate)
VALUES('2024-08-25 00:00:00.000', '2024-08-26 00:00:00.000', N'Feliz Independencia', 0.25);
INSERT INTO depoquick.dbo.Promotions
(ValidityDate_InitialDate, ValidityDate_FinalDate, Label, DiscountRate)
VALUES('2024-10-01 00:00:00.000', '2024-10-31 00:00:00.000', N'Octubre OFF', 0.1);

INSERT INTO depoquick.dbo.Deposits
(AirConditioning, Area, [Size], Name)
VALUES(0, N'A', N'PEQUEÑO', N'Central');
INSERT INTO depoquick.dbo.Deposits
(AirConditioning, Area, [Size], Name)
VALUES(1, N'B', N'MEDIANO', N'Empresarial');
INSERT INTO depoquick.dbo.Deposits
(AirConditioning, Area, [Size], Name)
VALUES(1, N'C', N'GRANDE', N'Premium');
INSERT INTO depoquick.dbo.Deposits
(AirConditioning, Area, [Size], Name)
VALUES(0, N'D', N'PEQUEÑO', N'Comercial');
INSERT INTO depoquick.dbo.Deposits
(AirConditioning, Area, [Size], Name)
VALUES(0, N'E', N'MEDIANO', N'Corporativo');
INSERT INTO depoquick.dbo.Deposits
(AirConditioning, Area, [Size], Name)
VALUES(1, N'A', N'GRANDE', N'Ejecutivo');

INSERT INTO depoquick.dbo.Reservations
(DepositId, ClientId, Date_InitialDate, Date_FinalDate, Status, Message, Price, RequestedAt)
VALUES(1, 2, '2024-06-09 00:00:00.000', '2024-06-11 00:00:00.000', 1, N'-', 100, '2024-06-09 20:34:04.251');
INSERT INTO depoquick.dbo.Reservations
(DepositId, ClientId, Date_InitialDate, Date_FinalDate, Status, Message, Price, RequestedAt)
VALUES(5, 2, '2024-06-18 00:00:00.000', '2024-06-22 00:00:00.000', 1, N'-', 300, '2024-06-09 20:34:22.220');
INSERT INTO depoquick.dbo.Reservations
(DepositId, ClientId, Date_InitialDate, Date_FinalDate, Status, Message, Price, RequestedAt)
VALUES(3, 2, '2024-05-29 00:00:00.000', '2024-06-13 00:00:00.000', 1, N'-', 1620, '2024-06-09 20:34:53.294');
INSERT INTO depoquick.dbo.Reservations
(DepositId, ClientId, Date_InitialDate, Date_FinalDate, Status, Message, Price, RequestedAt)
VALUES(2, 2, '2024-06-27 00:00:00.000', '2024-07-06 00:00:00.000', 1, N'-', 812, '2024-06-09 20:35:14.630');
INSERT INTO depoquick.dbo.Reservations
(DepositId, ClientId, Date_InitialDate, Date_FinalDate, Status, Message, Price, RequestedAt)
VALUES(3, 3, '2024-06-07 00:00:00.000', '2024-06-08 00:00:00.000', 1, N'-', 120, '2024-06-09 20:38:17.861');
INSERT INTO depoquick.dbo.Reservations
(DepositId, ClientId, Date_InitialDate, Date_FinalDate, Status, Message, Price, RequestedAt)
VALUES(2, 3, '2024-06-14 00:00:00.000', '2024-06-22 00:00:00.000', 1, N'-', 722, '2024-06-09 20:38:29.874');
INSERT INTO depoquick.dbo.Reservations
(DepositId, ClientId, Date_InitialDate, Date_FinalDate, Status, Message, Price, RequestedAt)
VALUES(5, 3, '2024-06-10 00:00:00.000', '2024-06-14 00:00:00.000', 1, N'-', 300, '2024-06-09 20:39:15.824');
INSERT INTO depoquick.dbo.Reservations
(DepositId, ClientId, Date_InitialDate, Date_FinalDate, Status, Message, Price, RequestedAt)
VALUES(1, 3, '2024-06-12 00:00:00.000', '2024-06-15 00:00:00.000', 1, N'-', 150, '2024-06-09 20:39:28.386');
INSERT INTO depoquick.dbo.Reservations
(DepositId, ClientId, Date_InitialDate, Date_FinalDate, Status, Message, Price, RequestedAt)
VALUES(4, 3, '2024-06-21 00:00:00.000', '2024-06-29 00:00:00.000', 1, N'-', 380, '2024-06-09 20:39:36.854');
INSERT INTO depoquick.dbo.Reservations
(DepositId, ClientId, Date_InitialDate, Date_FinalDate, Status, Message, Price, RequestedAt)
VALUES(4, 4, '2024-06-09 00:00:00.000', '2024-06-15 00:00:00.000', 0, N'-', 300, '2024-06-09 20:41:47.478');
INSERT INTO depoquick.dbo.Reservations
(DepositId, ClientId, Date_InitialDate, Date_FinalDate, Status, Message, Price, RequestedAt)
VALUES(5, 4, '2024-06-09 00:00:00.000', '2024-06-15 00:00:00.000', 0, N'-', 450, '2024-06-09 20:41:54.870');
INSERT INTO depoquick.dbo.Reservations
(DepositId, ClientId, Date_InitialDate, Date_FinalDate, Status, Message, Price, RequestedAt)
VALUES(3, 4, '2024-06-12 00:00:00.000', '2024-06-21 00:00:00.000', 0, N'-', 1026, '2024-06-09 20:42:03.328');
INSERT INTO depoquick.dbo.Reservations
(DepositId, ClientId, Date_InitialDate, Date_FinalDate, Status, Message, Price, RequestedAt)
VALUES(3, 4, '2024-06-14 00:00:00.000', '2024-06-25 00:00:00.000', 0, N'-', 1254, '2024-06-09 20:42:11.387');
INSERT INTO depoquick.dbo.Reservations
(DepositId, ClientId, Date_InitialDate, Date_FinalDate, Status, Message, Price, RequestedAt)
VALUES(2, 4, '2024-06-04 00:00:00.000', '2024-06-25 00:00:00.000', 0, N'-', 1795, '2024-06-09 20:42:22.613');
INSERT INTO depoquick.dbo.Reservations
(DepositId, ClientId, Date_InitialDate, Date_FinalDate, Status, Message, Price, RequestedAt)
VALUES(3, 2, '2024-06-13 00:00:00.000', '2024-06-28 00:00:00.000', -1, N'Lo sentimos, no es posible aprobar la reserva porque el depósito ya está reservado en esa fecha.', 1620, '2024-06-09 20:43:02.129');
INSERT INTO depoquick.dbo.Reservations
(DepositId, ClientId, Date_InitialDate, Date_FinalDate, Status, Message, Price, RequestedAt)
VALUES(3, 2, '2024-06-13 00:00:00.000', '2024-06-14 00:00:00.000', -1, N'Lo sentimos, el depósito ya no está disponible en esa fecha.', 120, '2024-06-09 20:43:28.991');
INSERT INTO depoquick.dbo.Reservations
(DepositId, ClientId, Date_InitialDate, Date_FinalDate, Status, Message, Price, RequestedAt)
VALUES(4, 2, '2024-06-15 00:00:00.000', '2024-06-18 00:00:00.000', 1, N'-', 150, '2024-06-09 20:43:41.557');
INSERT INTO depoquick.dbo.Reservations
(DepositId, ClientId, Date_InitialDate, Date_FinalDate, Status, Message, Price, RequestedAt)
VALUES(6, 2, '2024-06-05 00:00:00.000', '2024-06-18 00:00:00.000', 0, N'-', 1482, '2024-06-09 20:43:49.352');
INSERT INTO depoquick.dbo.Reservations
(DepositId, ClientId, Date_InitialDate, Date_FinalDate, Status, Message, Price, RequestedAt)
VALUES(3, 2, '2024-06-14 00:00:00.000', '2024-06-18 00:00:00.000', 1, N'-', 480, '2024-06-09 20:43:57.196');
INSERT INTO depoquick.dbo.Reservations
(DepositId, ClientId, Date_InitialDate, Date_FinalDate, Status, Message, Price, RequestedAt)
VALUES(4, 2, '2024-06-14 00:00:00.000', '2024-06-18 00:00:00.000', -1, N'Lo sentimos, ha habido un problema con su reserva.', 200, '2024-06-09 20:44:04.831');

INSERT INTO depoquick.dbo.Payments
(Status, ReservationId)
VALUES(N'capturado', 1);
INSERT INTO depoquick.dbo.Payments
(Status, ReservationId)
VALUES(N'capturado', 2);
INSERT INTO depoquick.dbo.Payments
(Status, ReservationId)
VALUES(N'capturado', 4);
INSERT INTO depoquick.dbo.Payments
(Status, ReservationId)
VALUES(N'capturado', 5);
INSERT INTO depoquick.dbo.Payments
(Status, ReservationId)
VALUES(N'capturado', 6);
INSERT INTO depoquick.dbo.Payments
(Status, ReservationId)
VALUES(N'capturado', 7);
INSERT INTO depoquick.dbo.Payments
(Status, ReservationId)
VALUES(N'capturado', 8);
INSERT INTO depoquick.dbo.Payments
(Status, ReservationId)
VALUES(N'capturado', 9);
INSERT INTO depoquick.dbo.Payments
(Status, ReservationId)
VALUES(N'capturado', 3);
INSERT INTO depoquick.dbo.Payments
(Status, ReservationId)
VALUES(N'reservado', 15);
INSERT INTO depoquick.dbo.Payments
(Status, ReservationId)
VALUES(N'reservado', 16);
INSERT INTO depoquick.dbo.Payments
(Status, ReservationId)
VALUES(N'capturado', 17);
INSERT INTO depoquick.dbo.Payments
(Status, ReservationId)
VALUES(N'capturado', 19);
INSERT INTO depoquick.dbo.Payments
(Status, ReservationId)
VALUES(N'reservado', 20);

INSERT INTO depoquick.dbo.Ratings
(Stars, Comment, DepositId, ReservationId)
VALUES(5, N'Excelente servicio', 3, 5);

INSERT INTO depoquick.dbo.Notifications
(Message, [Timestamp], ClientId)
VALUES(N'Su reserva del deposito 1 en las fechas 09/06/2024 a 11/06/2024 ha sido aprobada. ¡Gracias por confiar en nosotros!', '2024-06-09 20:37:02.935', 2);
INSERT INTO depoquick.dbo.Notifications
(Message, [Timestamp], ClientId)
VALUES(N'Su reserva del deposito 5 en las fechas 18/06/2024 a 22/06/2024 ha sido aprobada. ¡Gracias por confiar en nosotros!', '2024-06-09 20:37:03.441', 2);
INSERT INTO depoquick.dbo.Notifications
(Message, [Timestamp], ClientId)
VALUES(N'Su reserva del deposito 2 en las fechas 27/06/2024 a 06/07/2024 ha sido aprobada. ¡Gracias por confiar en nosotros!', '2024-06-09 20:37:05.419', 2);
INSERT INTO depoquick.dbo.Notifications
(Message, [Timestamp], ClientId)
VALUES(N'Su reserva del deposito 3 en las fechas 07/06/2024 a 08/06/2024 ha sido aprobada. ¡Gracias por confiar en nosotros!', '2024-06-09 20:40:36.291', 3);
INSERT INTO depoquick.dbo.Notifications
(Message, [Timestamp], ClientId)
VALUES(N'Su reserva del deposito 2 en las fechas 14/06/2024 a 22/06/2024 ha sido aprobada. ¡Gracias por confiar en nosotros!', '2024-06-09 20:40:37.625', 3);
INSERT INTO depoquick.dbo.Notifications
(Message, [Timestamp], ClientId)
VALUES(N'Su reserva del deposito 5 en las fechas 10/06/2024 a 14/06/2024 ha sido aprobada. ¡Gracias por confiar en nosotros!', '2024-06-09 20:40:38.328', 3);
INSERT INTO depoquick.dbo.Notifications
(Message, [Timestamp], ClientId)
VALUES(N'Su reserva del deposito 1 en las fechas 12/06/2024 a 15/06/2024 ha sido aprobada. ¡Gracias por confiar en nosotros!', '2024-06-09 20:40:43.505', 3);
INSERT INTO depoquick.dbo.Notifications
(Message, [Timestamp], ClientId)
VALUES(N'Su reserva del deposito 4 en las fechas 21/06/2024 a 29/06/2024 ha sido aprobada. ¡Gracias por confiar en nosotros!', '2024-06-09 20:40:43.999', 3);
INSERT INTO depoquick.dbo.Notifications
(Message, [Timestamp], ClientId)
VALUES(N'Su reserva del deposito 3 en las fechas 29/05/2024 a 13/06/2024 ha sido aprobada. ¡Gracias por confiar en nosotros!', '2024-06-09 20:44:45.065', 2);
INSERT INTO depoquick.dbo.Notifications
(Message, [Timestamp], ClientId)
VALUES(N'Su reserva del deposito 3 en las fechas 13/06/2024 a 28/06/2024 ha sido rechazada. Si pagaste, te devolveremos el dinero a tu cuenta.', '2024-06-09 20:45:06.509', 2);
INSERT INTO depoquick.dbo.Notifications
(Message, [Timestamp], ClientId)
VALUES(N'Su reserva del deposito 3 en las fechas 13/06/2024 a 14/06/2024 ha sido rechazada. Si pagaste, te devolveremos el dinero a tu cuenta.', '2024-06-09 20:45:25.040', 2);
INSERT INTO depoquick.dbo.Notifications
(Message, [Timestamp], ClientId)
VALUES(N'Su reserva del deposito 4 en las fechas 15/06/2024 a 18/06/2024 ha sido aprobada. ¡Gracias por confiar en nosotros!', '2024-06-09 20:45:27.640', 2);
INSERT INTO depoquick.dbo.Notifications
(Message, [Timestamp], ClientId)
VALUES(N'Su reserva del deposito 3 en las fechas 14/06/2024 a 18/06/2024 ha sido aprobada. ¡Gracias por confiar en nosotros!', '2024-06-09 20:45:29.022', 2);
INSERT INTO depoquick.dbo.Notifications
(Message, [Timestamp], ClientId)
VALUES(N'Su reserva del deposito 4 en las fechas 14/06/2024 a 18/06/2024 ha sido rechazada. Si pagaste, te devolveremos el dinero a tu cuenta.', '2024-06-09 20:45:59.240', 2);

INSERT INTO depoquick.dbo.LogEntries
(Message, [Timestamp], UserId)
VALUES(N'Ingresó al sistema', '2024-06-09 20:23:18.241', 1);
INSERT INTO depoquick.dbo.LogEntries
(Message, [Timestamp], UserId)
VALUES(N'Cerró sesión', '2024-06-09 20:32:33.661', 1);
INSERT INTO depoquick.dbo.LogEntries
(Message, [Timestamp], UserId)
VALUES(N'Ingresó al sistema', '2024-06-09 20:33:46.394', 2);
INSERT INTO depoquick.dbo.LogEntries
(Message, [Timestamp], UserId)
VALUES(N'Cerró sesión', '2024-06-09 20:35:32.784', 2);
INSERT INTO depoquick.dbo.LogEntries
(Message, [Timestamp], UserId)
VALUES(N'Ingresó al sistema', '2024-06-09 20:35:44.336', 1);
INSERT INTO depoquick.dbo.LogEntries
(Message, [Timestamp], UserId)
VALUES(N'Cerró sesión', '2024-06-09 20:37:24.940', 1);
INSERT INTO depoquick.dbo.LogEntries
(Message, [Timestamp], UserId)
VALUES(N'Ingresó al sistema', '2024-06-09 20:37:53.025', 3);
INSERT INTO depoquick.dbo.LogEntries
(Message, [Timestamp], UserId)
VALUES(N'Cerró sesión', '2024-06-09 20:40:06.523', 3);
INSERT INTO depoquick.dbo.LogEntries
(Message, [Timestamp], UserId)
VALUES(N'Ingresó al sistema', '2024-06-09 20:40:22.184', 1);
INSERT INTO depoquick.dbo.LogEntries
(Message, [Timestamp], UserId)
VALUES(N'Cerró sesión', '2024-06-09 20:41:11.894', 1);
INSERT INTO depoquick.dbo.LogEntries
(Message, [Timestamp], UserId)
VALUES(N'Ingresó al sistema', '2024-06-09 20:41:35.340', 4);
INSERT INTO depoquick.dbo.LogEntries
(Message, [Timestamp], UserId)
VALUES(N'Cerró sesión', '2024-06-09 20:42:30.305', 4);
INSERT INTO depoquick.dbo.LogEntries
(Message, [Timestamp], UserId)
VALUES(N'Ingresó al sistema', '2024-06-09 20:42:38.994', 2);
INSERT INTO depoquick.dbo.LogEntries
(Message, [Timestamp], UserId)
VALUES(N'Cerró sesión', '2024-06-09 20:44:19.965', 2);
INSERT INTO depoquick.dbo.LogEntries
(Message, [Timestamp], UserId)
VALUES(N'Ingresó al sistema', '2024-06-09 20:44:32.939', 1);
INSERT INTO depoquick.dbo.LogEntries
(Message, [Timestamp], UserId)
VALUES(N'Cerró sesión', '2024-06-09 21:56:15.438', 1);
INSERT INTO depoquick.dbo.LogEntries
(Message, [Timestamp], UserId)
VALUES(N'Ingresó al sistema', '2024-06-09 21:56:25.165', 3);
INSERT INTO depoquick.dbo.LogEntries
(Message, [Timestamp], UserId)
VALUES(N'Agregó valoración de la reserva 5', '2024-06-09 21:56:47.110', 3);
INSERT INTO depoquick.dbo.LogEntries
(Message, [Timestamp], UserId)
VALUES(N'Cerró sesión', '2024-06-09 21:57:07.182', 3);
INSERT INTO depoquick.dbo.LogEntries
(Message, [Timestamp], UserId)
VALUES(N'Ingresó al sistema', '2024-06-09 21:57:16.431', 2);
INSERT INTO depoquick.dbo.LogEntries
(Message, [Timestamp], UserId)
VALUES(N'Cerró sesión', '2024-06-09 21:57:51.028', 2);