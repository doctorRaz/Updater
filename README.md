# Updater
[В начале разработки]
>Disclaimer 

## Зачем это вот все?

Разработчик пишет приложения, иногда в этих приложениях обнаруживаются баги, порой пользователи предлагают хорошие идеи по улучшению программы.\
Рано или поздно разработчик все это делает.\
Возникает вопрос, как пользователь узнает, что программа улучшена?\
На сайт программы пользователь не ходит, скачал и забыл, просто пользуется программой.\
Сейчас когда к интернету подключена каждая скороварка и утюг я считаю это неправильным.\
Программы должны быть актуальными и безопасными.\
Собственно для этого и разрабатывается эта библиотека.

## Почему это будет работать?

Сама идея не нова.\
Windows не позволит удалить или переместить загруженный файл, будь то *.exe или *.dll, но она даст переименовать файл.\
т.е. запущенное приложение может обновить само себя:
* меняет расширение своих файлов, например на *.bak
* новые одноименные файлы копирует на место переименованных
* при следующем запуске, загрузятся обновленные файлы приложения, старые *.bak можно проблемно удалить.

## Как это работает?

1. **Wrapper** на стороне разработчика:
* Делает опись файлов которые подлежат обновлению в файл XML
* Упаковывает в шифрованный zip архив (шифрование необходимо потому, что некоторые корпоративные сервисы не пропускают закачку *.exe, *.dll напрямую, хотя конечно и это не панацея)
Разработчик закачивает на сетевой ресурс *.XML и *.zip
2. UnWrapper на стороне пользователя 
* Скачивает *.XML
* сравнивает версии модулей приложения и те что записаны в *.XML
* если в *.XML версии новее:
	* уведомляет пользователя (опционально)
	* скачивает с сетевого ресурса *.zip, распаковывает
	* меняет расширения обновляемым модулям
	* заменяет обновляемые модули новыми
	* уведомляет пользователя, что нужна перезагрузка

 

`пишется еще`