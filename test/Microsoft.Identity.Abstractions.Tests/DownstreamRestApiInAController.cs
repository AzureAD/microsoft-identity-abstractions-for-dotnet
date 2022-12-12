// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Identity.Abstractions.DownstreamRestApi.Tests
{
    internal class Todo
    {
        public Todo(string description, int id = -1) { Id = id; Description = description; }

        public int Id { get; set; }

        public string Description { get; set; }
    }

    internal class ActionResult
    {
    }

    internal class DownstreamRestApiController
    {
        private IDownstreamRestApi _downstreamWebApi = new CustomDownstreamRestApi();
        private const string ServiceName = "TodoList";

        private ActionResult View(object? _) { return new ActionResult(); }

        private ActionResult NotFound() { return new ActionResult(); }

        private ActionResult RedirectToAction(string _) { return new ActionResult(); }

        // GET: TodoList. Get all the items in the list
        public async Task<ActionResult> Index()
        {
            var value = await _downstreamWebApi.GetForUserAsync<IEnumerable<Todo>>(ServiceName,
                options => options.RelativePath = "api/todolist");
            return View(value);
        }

        // GET: TodoList/Details/5 (get one item in the list)
        public async Task<ActionResult> Details(int id)
        {
            var value = await _downstreamWebApi.GetForUserAsync<Todo>(
                ServiceName,
                options => options.RelativePath = $"api/todolist/{id}");

            return View(value);
        }

        // GET: TodoList/Create a new item
        public ActionResult Create()
        {
            Todo todo = new Todo("blah");

            return View(todo);
        }

        // POST: TodoList/Create
        public async Task<ActionResult> Create(/*[Bind("Title,Owner")]*/ Todo todo)
        {
            await _downstreamWebApi.PostForUserAsync<Todo, Todo>(ServiceName, todo,
                o => o.RelativePath = "api/todolist");

            return RedirectToAction("Index");
        }

        // GET: TodoList/Edit/5
        public async Task<ActionResult> GetInOrderToEdit(int id)
        {
            Todo? todo = await _downstreamWebApi.GetForUserAsync<Todo>(
                ServiceName,
                o => o.RelativePath = $"api/todolist/{id}");

            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // POST: TodoList/Edit/5
        public async Task<ActionResult> Edit(int id, /*[Bind("Id,Title,Owner")]*/ Todo todo)
        {
            await _downstreamWebApi.CallRestApiForUserAsync<Todo, Todo>(
                ServiceName,
                todo,
                options =>
                {
                    options.HttpMethod = HttpMethod.Patch;
                    options.RelativePath = $"api/todolist/{todo.Id}";
                });

            return RedirectToAction("Index");
        }

        // GET: TodoList/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            Todo? todo = await _downstreamWebApi.GetForUserAsync<Todo>(
                ServiceName,
                options  => options.RelativePath = $"api/todolist/{id}");

            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // POST: TodoList/Delete/5
        public async Task<ActionResult> Delete(/*[Bind("Id,Title,Owner")]*/ Todo todo)
        {
            await _downstreamWebApi.DeleteForUserAsync<Todo>(
                ServiceName,
                todo,
                options =>
                {
                    options.RelativePath = $"api/todolist/{todo.Id}";
                });

            return RedirectToAction("Index");
        }
    }
}
