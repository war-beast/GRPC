# Микросервисы общающиеся по GRPC

Знакомство

### Проекты:

Grpc - внешний API (API-гейт)

SimpleGrpcService - простой микросервис приветствия

IntermediateService - промежуточный сервис между API и сервисом приветствия (timeout propagation)

JsonTranscoding - сервис с сериализацией запросов в формат JSON

FileUploader - работа с файлами, хранящимися в MinIO
