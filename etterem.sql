-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2026. Okt 12. 09:38
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `etterem`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `rendeles`
--

CREATE TABLE `rendeles` (
  `id` int(11) NOT NULL,
  `dish` varchar(40) NOT NULL,
  `description` text DEFAULT NULL,
  `orderTime` datetime NOT NULL,
  `updateTime` datetime NOT NULL,
  `vendegId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- A tábla adatainak kiíratása `rendeles`
--

INSERT INTO `rendeles` (`id`, `dish`, `description`, `orderTime`, `updateTime`, `vendegId`) VALUES
(1, 'Gulyásleves', 'Extra csípős kérésre, kenyérrel.', '2025-05-02 12:30:00', '2025-05-02 12:30:00', 1),
(2, 'Rántott sajt', 'Hasábburgonyával és tartármártással.', '2025-05-14 19:15:00', '2025-05-14 19:15:00', 2),
(3, 'Halászlé', 'Szegedi módra, csípős paprikával.', '2025-05-26 13:00:00', '2025-05-26 13:00:00', 3),
(4, 'Túrós csusza', 'Tepertővel, dupla adag tejföllel.', '2025-06-08 20:40:00', '2025-06-08 20:40:00', 5),
(5, 'Somlói galuska', 'Desszert, extra csokiöntettel.', '2025-06-19 21:20:00', '2025-06-19 21:20:00', 6);

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `rendeles`
--
ALTER TABLE `rendeles`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_rendeles_vendeg` (`vendegId`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `rendeles`
--
ALTER TABLE `rendeles`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `rendeles`
--
ALTER TABLE `rendeles`
  ADD CONSTRAINT `fk_rendeles_vendeg` FOREIGN KEY (`vendegId`) REFERENCES `vendeg` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
