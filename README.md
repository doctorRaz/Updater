# Updater
[В начале разработки]
>Disclaimer 

## Зачем это вот все
---
Разработчик пишет приложения, иногда в этих приложениях обнаруживаются баги, иногда пользователи предлагают хорошие идеи по улучшению.
Рано или поздно разработчик все это реализует в своей программе.
Возникает вопрос, как пользователь узнает, что программа улучшена?
На сайт программы пользователь не ходит, скачал и забыл, просто пользуется программой.
В во время когда к интернету подключена каждая скороварка и утюг я считаю это неправильным.\
Программы должны быть актуальными и безопасными.\
Собственно для этого и разрабатывается эта библиотека.

## Почему это будет работать
---


1. **Wrapper** делает опись файлов которые подлежат обновлению в файл XML
* Упаковывает в шифрованный zip архив (шифрование необходимо для того, что некоторые корпоративные сервисы не пропускают закачку *.exe, *.dll)

2. UnWrapper скачивает 
Обновление программ\
Как это работает:
>Собирается 
два модуля
* писатель записывает xml и пакует  файлы в zip
* читатель читает XML проверяет версии и распаковывает обновления по путям
*
* 
*
*
