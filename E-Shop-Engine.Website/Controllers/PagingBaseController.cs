﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using AutoMapper;
using X.PagedList;

namespace E_Shop_Engine.Website.Controllers
{
    public class PagingBaseController : Controller
    {
        protected IPagedList<T> IEnumerableToPagedList<T>(IEnumerable<T> model, int? page, int pageSize = 25)
        {
            int pageNumber = page ?? 1;
            IPagedList<T> pagedModel = new PagedList<T>(model, pageNumber, pageSize);
            return pagedModel;
        }

        protected IPagedList<TDestination> IEnumerableToPagedList<TSource, TDestination, TSort>(IEnumerable<TSource> model, Expression<Func<TSource, TSort>> sortCondition, int? page, int pageSize = 25, bool descending = false)
        {
            int pageNumber = page ?? 1;
            IPagedList<TSource> pagedModel;
            if (descending)
            {
                pagedModel = model.AsQueryable().OrderByDescending(sortCondition).ToPagedList(pageNumber, pageSize);
            }
            else
            {
                pagedModel = model.AsQueryable().OrderBy(sortCondition).ToPagedList(pageNumber, pageSize);
            }
            IEnumerable<TDestination> mappedModel = Mapper.Map<IEnumerable<TDestination>>(pagedModel);
            IPagedList<TDestination> viewModel = new StaticPagedList<TDestination>(mappedModel, pagedModel.GetMetaData());
            return viewModel;
        }

        protected IPagedList<TDestination> IQueryableToPagedList<TSource, TDestination, TSort>(IQueryable<TSource> model, Expression<Func<TSource, TSort>> sortCondition, int? page, int pageSize = 25, bool descending = false)
        {
            int pageNumber = page ?? 1;
            IPagedList<TSource> pagedModel;
            if (descending)
            {
                pagedModel = model.OrderByDescending(sortCondition).ToPagedList(pageNumber, pageSize);
            }
            else
            {
                pagedModel = model.OrderBy(sortCondition).ToPagedList(pageNumber, pageSize);
            }
            IEnumerable<TDestination> mappedModel = Mapper.Map<IEnumerable<TDestination>>(pagedModel);
            IPagedList<TDestination> viewModel = new StaticPagedList<TDestination>(mappedModel, pagedModel.GetMetaData());
            return viewModel;
        }
    }
}