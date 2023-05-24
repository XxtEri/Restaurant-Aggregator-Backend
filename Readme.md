В данном проекте реализован сервис-агрегатор ресторанов доставки еды. Сервис предусматривает распределенную архитектуру, некоторую ролевую модель и уведомления в реальном времени.

<h2>Что важно знать о проекте?</h2>
Для обеспечения слабой связности и масштабируемости системы, все сервисы работают с собственной БД. Для реляционных хранилищ используется PostgreSQL. Добавлены миграции

<h2>Что необходимо для сборки проекта?</h2>
В проекте есть файл docker-compose.yml, который необходимо запустить, чтобы создать docker образы баз данных. Далее в своей IDE нужно подключить базы данных. Также следует запустить docker образ RabbitMQ для работы с уведомлениями, написав в терминале/консоли: docker run -d --hostname my-rabbit --name some-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management

<h2>Что есть в проекте?</h2>

В проекте разработано несколько компонентов:
- Auth
- Backend
- Admin panel
- Notifications

В проекте предусмотрены такие роли:
- Admin
- Customer
- Manager
- Cook
- Courier

<h3>Auth компонент</h3>
Сервис отвечает за хранение информации о пользователях и работу с ними. Выполняет такие функции, как:

- регистрация пользователя (с ролью Customer и работника, как Customer)
- аутентификация
- выдача токенов авторизации
- рефреш токена
- выход из аккаунта
- получение информации об аккаунте пользователя (с ролью Customer)
- изменения информации профиля пользователя (с ролью Customer)
- изменение пароля аккаунта пользователя (с ролью Customer)

<h3>Backend компонент</h3>
Основной сервис, инкапсулирующий бизнес-логику приложения. Для всех доступны такие функции:

- просмотр ресторанов с пагинацией и поиском по названию
- прсомотр информации о конкретном ресторане по id
- добавление меню в ресторан
- просмотр меню у конкретного ресторана
- просмотр списка блюд конкретного ресторана с возможностью пагинации, фильтрациями/сортировками по категории, по вегетарианским блюдам, сортировка блюд (по имени от А-Я, по имени от Я-А, по возрастанию цены, по убыванию цены, по возрастанию  рейтинга, по убыванию  рейтинга)
- получение списка блюд в конкретном меню ресторана
- просмотр детальной информации о блюде (с рейтингом)

Для менеджеров доступны такие функции:

- создание меню в ресторане
- добавление блюда в меню ресторана
- просмотр всех заказов в их ресторане с возможность пагинации и фильтрации по статусам, сортировкой по дате создания, дате доставки, а также поиском по номеру заказа

Для покупателей доступны такие функции:

- проверка на то, можно ли поставить рейтинг покупателю на то или иное блюдо
- возможность выставить рейтинг блюду
- добавить блюдо в корзину
- удалить блюдо из корзины
- увеличить/уменьшить количество порций блюда в корзине
- просмотр текущей корзины
- возможность оформить заказ, но в случае если в корзине блюда только из одного ресторана
- полная очистка корзины
- отменить заказ (изменение статуса на Cancelled)
- посмотреть историю своих заказов с пагинацией, фильтрации по дате заказа
- возможность повторить один из прошлых заказов
- возможность посмотреть текущий заказ 
- посмотреть детальную информацию о заказе по его номеру

Для поваров доступны такие функции:

- просмотр всех заказов в статусе  “Created” созданные в их ресторане (с возможностью пагинации и сортировки по дате создания и дате доставки)
- просмотр истории выполненных им заказов
- просмотр заказов, взятые на готовку
- смена статуса заказа на “Kitchen”, “Packaging” и "WaitingCourier"

**ВАЖНО**: Взятые заказы повар может взять на кухню, изменив статус заказа на “Kitchen”. Заказ пакуется там же на кухне, изменяет статус заказа повар на “Packaging”, как только заказ будет готов для доставки высталяется статус "WaitingCourier"

Для курьеров доступны такие функции:

- просмотр заказов со статусом “Delivery”
- просмотр истории выполненных заказов
- просмотр взятого заказа 
- изменить статус заказа на Delivered
- отменить заказ, если не смог доставить покупателю (изменить статус заказа на Cancelled)

**ВАЖНО**: Пользователи с ролью “Courier” могут взять только один заказ в единицу времени для доставки покупателю.

<h3>Admin panel компонент</h3>
Приложение - административная панель. Это приложение используется администраторами системы для выполнения следующих функций:
- CRUD ресторанов
- CRUD менеджеров, поваров, курьеров
- бан каких-либо пользователей в системе

В приложении при запуске автоматически создается admin пользователь, если такого нет. После на странице авторизации следует ввести следующие данные для входа в admin panel:
- login: admin@gmail.com
- password: Admin1

Для входа используется cookie авторизация

После успешного входа на странице "Рестораны" у администраторы есть возможность:
- создать новый ресторан
- посмотреть подробную информацию о нем
- изменить данные о ресторане
- удалить ресторан из базы данных
- искать ресторан по названию

После успешного входа на странице "Пользователи" у администраторы есть возможность:
- создать нового пользователя без ролей
- посмотреть подробную информацию о пользователе
- изменить данные о пользователе
- удалить пользователя
- добавить/удалить роль Cook, Manager или Courier
- назначить пользователя с ролью Cook или Manager на работу в какой-то ресторан
- забанить пользователя

На всех view с введением/изменением данных присутсвует валидация.

**ВАЖНО**: если указывать id не существующего ресторана при назначении на работу в ресторан повара или менеджера, то ошибка не выведется, но и id не добавиться к менеджеру или повару. (Не успела добавить вывод модального окна с ошибкой, но проверка на такой случай есть)

<h3>Notifications компонент</h3>

Сервис отвечает за работу с уведомлениями в реальном времени, используя RabbitMQ. В нем есть небольшая конфигурация для подключения возможности использовать Web-сокеты и библиотеку SignalR.

Уведомления приходят пользователю с ролью Customer тогда, когда изменеяется статус заказа
