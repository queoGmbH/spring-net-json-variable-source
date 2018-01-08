# spring-net-json-variable-source

As resharper does not get along with the default properties files for spring and because we are used to configure things in json format, we decided to use a json-variable-source for spring.net.

This comes with some very positiv side effects:
- Validation of the (json)-config
- Intellisense in Visual Studio when writing the json-file
- complex configuration objects
- No more Resharper errors when opening the properties file
- maybe more ...


This is a sample the json-config-file ...


![json-config](json.png)


... for the specified json-schema ...


![json-schema-file](schema.png)


With intellisense ...


![json-schema-file](intellisense.png)


... and validation ...
... for missing properties ...


![json-schema-file](validation1.png)


![json-schema-file](validation2.png)


... for invalid types ...


![Validation of type](validation3.png)