-- 1. CLEANUP
DROP TABLE IF EXISTS WorkoutLogs;

DROP TABLE IF EXISTS Workouts;

DROP TABLE IF EXISTS Exercises;

DROP TABLE IF EXISTS Categories;

-- 2. SCHEMA (English)
CREATE TABLE
  Categories (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL
  );

CREATE TABLE
  Exercises (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    CategoryId INTEGER,
    FOREIGN KEY (CategoryId) REFERENCES Categories (Id)
  );

CREATE TABLE
  Workouts (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT,
    Date TEXT -- ISO8601 strings
  );

CREATE TABLE
  WorkoutLogs (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    WorkoutId INTEGER,
    ExerciseId INTEGER,
    Sets INTEGER,
    Reps INTEGER,
    Weight REAL,
    FOREIGN KEY (WorkoutId) REFERENCES Workouts (Id),
    FOREIGN KEY (ExerciseId) REFERENCES Exercises (Id)
  );

-- 3. SEED CATEGORIES (Bulgarian Strings)
INSERT INTO
  Categories (Name)
VALUES
  ('Силови'),
  ('Кардио'),
  ('Гъвкавост');

-- 4. SEED EXERCISES (Bulgarian Strings)
INSERT INTO
  Exercises (Name, CategoryId)
VALUES
  ('Лежанка', 1),
  ('Клякания', 1),
  ('Мъртва тяга', 1),
  ('Раменна преса', 1),
  ('Гребане', 1),
  ('Бягане', 2),
  ('Колоездене', 2),
  ('Скачане на въже', 2),
  ('Бърпита', 2),
  ('Гребен тренажор', 2),
  ('Йога', 3),
  ('Разтягане', 3),
  ('Планк', 3),
  ('Поза дете', 3),
  ('Котка-крава', 3);

-- 5. SEED 6 WORKOUTS (Spread over last 3 months)
INSERT INTO
  Workouts (Id, Name, Date)
VALUES
  (1, 'Януарска Сила', datetime ('now', '-75 days')),
  (2, 'Зимно Кардио', datetime ('now', '-60 days')),
  (
    3,
    'Февруарски Микс',
    datetime ('now', '-45 days')
  ),
  (
    4,
    'Сила и Мобилност',
    datetime ('now', '-30 days')
  ),
  (
    5,
    'Пролетна Подготовка',
    datetime ('now', '-15 days')
  ),
  (
    6,
    'Интензивен Финал',
    datetime ('now', '-2 days')
  );

-- 6. SEED WORKOUT LOGS (2 to 6 per workout)
-- Workout 1 (4 logs)
INSERT INTO
  WorkoutLogs (WorkoutId, ExerciseId, Sets, Reps, Weight)
VALUES
  (1, 1, 3, 10, 60),
  (1, 2, 3, 12, 80),
  (1, 3, 1, 5, 100),
  (1, 13, 3, 60, 0);

-- Workout 2 (2 logs)
INSERT INTO
  WorkoutLogs (WorkoutId, ExerciseId, Sets, Reps, Weight)
VALUES
  (2, 6, 1, 1, 0),
  (2, 9, 4, 15, 0);

-- Workout 3 (5 logs)
INSERT INTO
  WorkoutLogs (WorkoutId, ExerciseId, Sets, Reps, Weight)
VALUES
  (3, 4, 3, 8, 40),
  (3, 5, 3, 10, 50),
  (3, 8, 5, 50, 0),
  (3, 11, 1, 1, 0),
  (3, 12, 2, 1, 0);

-- Workout 4 (3 logs)
INSERT INTO
  WorkoutLogs (WorkoutId, ExerciseId, Sets, Reps, Weight)
VALUES
  (4, 2, 4, 8, 85),
  (4, 13, 3, 45, 0),
  (4, 15, 2, 12, 0);

-- Workout 5 (6 logs)
INSERT INTO
  WorkoutLogs (WorkoutId, ExerciseId, Sets, Reps, Weight)
VALUES
  (5, 1, 5, 5, 90),
  (5, 3, 3, 3, 120),
  (5, 5, 3, 12, 55),
  (5, 7, 1, 1, 0),
  (5, 10, 1, 1, 0),
  (5, 14, 2, 1, 0);

-- Workout 6 (4 logs)
INSERT INTO
  WorkoutLogs (WorkoutId, ExerciseId, Sets, Reps, Weight)
VALUES
  (6, 4, 4, 10, 45),
  (6, 9, 5, 20, 0),
  (6, 13, 4, 60, 0),
  (6, 8, 3, 100, 0);