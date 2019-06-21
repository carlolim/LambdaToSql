# LambdaToSql
[![Build Status](https://travis-ci.org/carlolim/LambdaToSql.svg?branch=master)](https://travis-ci.org/carlolim/LambdaToSql)

LambdaToSql is library that converts lambda expression to sql string. 

## Overview


### Usage

```c#
using LambdaToSql;

public class MyClass
{
    private readonly ExpressionTranslator expressionTranslator;

    MyClass()
    {
        expressionTranslator = new ExpressionTranslator();
    }

    private EntityDetails EntityDetails<T>() where T : new()
    {
        var entity = new T();
        return entity.GetEntityDetails();
    }
        

    private QueryResult WhereClause<TEntity>(Expression<Func<TEntity, bool>> pred) where TEntity : class
    {
        return expressionTranslator.TranslateToQueryResult(pred);
    }
}
```
