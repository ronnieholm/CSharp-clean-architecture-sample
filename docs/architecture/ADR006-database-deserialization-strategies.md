# ADR006: Database deserialization strategies

Status: Accepted and active.

## Context

- Depends on shape of data
- How many passes over data
- Database overhead with single call vs multiple calls in parallel (multiple tasks, error handling)
- Does database support multiple result set in query response?
- Overhead of transferring extra data vs. network latency

```
a_id | a_c1 | a_c2 | b_id | b_c1 | b_c2 | c_id | c_c1 | c_c2
-----------------------------------------------------------
  x      x     x     y        y     y      z      z      z
  x      x     x     y        y     y      z1     z1     z1
  x      x     x     y1       y1    y1     z2     z2     z2
  x      x     x     y1       y1    y1     z3     z3     z3
```

a can have zero or many or b. b can have zero or many of c. This resembles an
aggregate root with a child and the child itself having children.

select s.id sid, s.title, s.description sdescription, s.created_at screated_at, s.updated_at supdated_at,
       t.id tid, t.title, t.description tdescription, t.created_at tcreated_at, t.updated_at tupdated_at
from stories s
left join tasks t on s.id == t.story_id
where s.id = 'e97abde2-b04c-4142-8599-83dee17341a8'

A query response would repeat the same data for x, y, y1 and we would skip it
doing parsing.

We can define an algorithm for how to parse a hierarchy of n levels deeps and
reuse that algorithm across queries.

Queries a typically single aggregate only, so a's would repeat, but we can
imagine the algorithm as it we'd returned multiple aggregates. Just to make it
more generally applicable.

In practice, queries returning multiple aggregate would typically not retain the
shape as above but return a custom shape based on the nature of the query.

It's a top-up parsing technique (start at the root), where the tree has been
flattened.

Simplify into where blank cell means repeated from closest above cell.

```
a_id | b_id | c_id
------------------
x_1    y_1    z_1
              z_2
       y_2    z_3
              z_4
x_2              
```

xx

## Decision



## Consequences

