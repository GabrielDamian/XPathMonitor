INSERT INTO users (username, password_hash) VALUES
('utilizator1', 'hash_parola_1'),
('utilizator2', 'hash_parola_2'),
('utilizator3', 'hash_parola_3');

INSERT INTO links (user_id, url, description) VALUES
(1, 'https://www.exemplu.com/produs1', 'Descriere produs 1'),
(1, 'https://www.exemplu.com/produs2', 'Descriere produs 2');

INSERT INTO links (user_id, url, description) VALUES
(2, 'https://www.exemplu.com/produs3', 'Descriere produs 3'),
(2, 'https://www.exemplu.com/produs4', 'Descriere produs 4');

INSERT INTO prices (link_id, price, currency) VALUES
(1, 100.50, 'RON'),
(1, 99.99, 'RON');

INSERT INTO prices (link_id, price, currency) VALUES
(2, 199.99, 'USD'),
(2, 189.99, 'USD');

INSERT INTO prices (link_id, price, currency) VALUES
(3, 49.99, 'EUR'),
(3, 45.99, 'EUR');



SELECT * FROM users;
SELECT * FROM links;
SELECT * FROM prices;
