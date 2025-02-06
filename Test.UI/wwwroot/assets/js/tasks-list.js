/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
var __webpack_exports__ = {};
/*!*******************************************!*\
  !*** ./resources/assets/js/tasks-list.js ***!
  \*******************************************/


(function ($) {
  //data-table
  $('#tasks-table').DataTable({
    language: {
      searchPlaceholder: 'Search...',
      sSearch: ''
    },
    pageLength: 10
  }); //file upload

  $('document').on("ready", function () {
    var $file = $('#task-file-input'),
        $label = $file.next('label'),
        $labelText = $label.find('span'),
        $labelRemove = $('i.remove'),
        labelDefault = $labelText.text(); // on file change

    $file.on('change', function (event) {
      var fileName = $file.val().split('\\').pop();

      if (fileName) {
        $labelText.text(fileName);
        $labelRemove.show();
      } else {
        $labelText.text(labelDefault);
        $labelRemove.hide();
      }
    }); // Remove file   

    $labelRemove.on('click', function (event) {
      $file.val("");
      $labelText.text(labelDefault);
      $labelRemove.hide();
      console.log($file);
    });
  }); // Select2 

  $('.select2').select2({
    minimumResultsForSearch: Infinity
  }); //select2 with indicator

  function selectStatus(status) {
    if (!status.id) {
      return status.text;
    }

    var $status = $('<span class="status-indicator projects">' + status.text + '</span>');
    var $statusText = $status.text().split(" ").join("").toLowerCase();

    if ($statusText === "inprogress") {
      $status.addClass("in-progress");
    } else if ($statusText === "onhold") {
      $status.addClass("on-hold");
    } else if ($statusText === "completed") {
      $status.addClass("completed");
    } else {
      $status.addClass("empty");
    }

    return $status;
  }

  ;
  $(".select2-status-search").select2({
    templateResult: selectStatus,
    templateSelection: selectStatus,
    escapeMarkup: function escapeMarkup(s) {
      return s;
    }
  });
})(jQuery); //todo task


var subTaskContainer = document.querySelector('.sub-list-container');

if (subTaskContainer) {
  //delete task
  var deleteSubTask = function deleteSubTask($e) {
    subTaskContainer.removeChild($e.target.parentElement);
  }; //mark all as completed vice verca


  var markAllCompleted = function markAllCompleted() {
    var allTasks = subTaskContainer.children;

    if (count % 2 != 0) {
      for (var i = 0; i < allTasks.length; i++) {
        allTasks[i].classList.remove('task-completed');
      }
    } else {
      for (var _i = 0; _i < allTasks.length; _i++) {
        allTasks[_i].classList.add('task-completed');
      }
    }

    count++;
  }; //remove all tasks


  var removeAllTasks = function removeAllTasks() {
    subTaskContainer.innerHTML = ' ';
  }; //add new task


  var addNewTask = function addNewTask() {
    errorNote.innerText = "";
    var newSubTask = taskCopy.cloneNode(true);
    newSubTask.classList.remove('task-completed');
    var newTaskText = subTaskInput.value;

    if (newTaskText.length !== 0) {
      subTaskContainer.appendChild(newSubTask);
      newSubTask.children[0].children[1].innerText = newTaskText;
      subTaskInput.value = "";
    } else {
      errorNote.innerText = "Please Enter Valid Input";
    }
  }; //mark task as completed


  var taskCompleted = function taskCompleted($e) {
    var currentSubList = $e.target;
    var subListParent = currentSubList.parentElement.parentElement;

    if (subListParent.classList.contains('task-completed')) {
      subListParent.classList.remove('task-completed');
    } else {
      subListParent.classList.add('task-completed');
    }
  };

  var subTaskElement = document.querySelector('.sub-list-item');
  var addSubTaskBtn = document.querySelector('#addTask');
  var subTaskInput = document.querySelector('#subTaskInput');
  var errorNote = document.querySelector('#errorNote');
  var deleteAllTasks = document.querySelector('#deleteAllTasks');
  var completedAllBtn = document.querySelector('#completedAll');
  setTimeout(function () {
    setInterval(function () {
      var deleteBtn = document.querySelectorAll('.delete-main');

      for (var i = 0; i < deleteBtn.length; i++) {
        deleteBtn[i].addEventListener('click', deleteSubTask);
      }
    }, 10);
  }, 1);
  var count = 0;
  var taskCopy = subTaskElement.cloneNode(true);
  completedAllBtn.addEventListener('click', markAllCompleted); // mark all completed

  deleteAllTasks.addEventListener('click', removeAllTasks); //delete all tasks

  addSubTaskBtn.addEventListener('click', addNewTask); //create new task
} //reply to a comment/review


function replay() {
  var replayButtom = document.querySelectorAll('.reply a'); // Creating Div

  var Div = document.createElement('div');
  Div.setAttribute('class', "comment mt-5 d-grid w-60"); // creating textarea

  var textArea = document.createElement('textarea');
  textArea.setAttribute('class', "form-control");
  textArea.setAttribute('rows', "5");
  textArea.innerText = "Your Comment"; // creating Cancel buttons

  var cancelButton = document.createElement('button');
  cancelButton.setAttribute('class', "btn btn-danger");
  cancelButton.innerText = "Cancel";
  var buttonDiv = document.createElement('div');
  buttonDiv.setAttribute('class', "btn-list ms-auto mt-2"); // Creating submit button

  var submitButton = document.createElement('button');
  submitButton.setAttribute('class', "btn btn-success ms-3");
  submitButton.innerText = "Submit"; // appending text are to div

  Div.append(textArea);
  Div.append(buttonDiv);
  buttonDiv.append(cancelButton);
  buttonDiv.append(submitButton);
  replayButtom.forEach(function (element, index) {
    element.addEventListener('click', function () {
      var replay = $(element).parent();
      replay.append(Div);
      cancelButton.addEventListener('click', function () {
        Div.remove();
      });
    });
  });
}

replay();
/******/ })()
;