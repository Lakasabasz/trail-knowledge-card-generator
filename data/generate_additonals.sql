drop table if exists node_posts;

create table node_posts
select postid
from points_in_line pil
group by postid
having count(linenr) > 1;

drop table if exists neighbour_map;

create table neighbour_map
select pil1.postid post_a, pil2.postid post_b, pil1.linenr linenr, pil1.kilometer kilometer_a, pil2.kilometer kilometer_b, abs(pil1.kilometer - pil2.kilometer) 'distance'
from points_in_line pil1 inner join points_in_line pil2 on pil1.postid != pil2.postid and pil1.linenr = pil2.linenr and pil1.postid < pil2.postid
where pil1.postid in (select * from node_posts) and pil2.postid in (select * from node_posts);
