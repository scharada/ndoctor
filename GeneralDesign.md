# General design #

The application is built on the MVVM pattern and supports plugins.

## Concepts ##

  * Component: a component is a piece of business logic. It consumes the entities nHibernates provides, process the data depending on business logic and gives the result using DTO objects.
  * nHibernate: it makes the link between the domain and the database. It hides the database implementation.

## Plugins ##

A plugin is a piece of business logic. It contains the GUI and the component to manipulate the business data.

Each plugin should **NOT** know about the entities and nHibernate. All the data work is done thought components.

## Schema ##

![https://docs.google.com/drawings/pub?id=19iMsxDBIwGX0Pa48jpxdX1gqahMZeG0Xduqrthc12qc&w=960&h=720&cht=bvg&nonsense=something_that_ends_with.png](https://docs.google.com/drawings/pub?id=19iMsxDBIwGX0Pa48jpxdX1gqahMZeG0Xduqrthc12qc&w=960&h=720&cht=bvg&nonsense=something_that_ends_with.png)