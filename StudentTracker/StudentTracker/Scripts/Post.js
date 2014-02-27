function BlogViewModel() {
    var self = this;
    self.modelTitle = ko.observable();

    self.totalPosts = ko.observableArray();

    self.totalCategories = ko.observableArray();

    self.totalTags = ko.observableArray();

    self.post = ko.observable();

    self.category = ko.observable();

    self.tag = ko.observable();

    self.IsPostEdit = ko.observable(false);

    self.IsCategoryEdit = ko.observable(false);

    self.IsTagEdit = ko.observable(false);

    self.sendCategories = ko.observable();
    self.sendTag = ko.observable();


    self.EditPost = function (post) {
        self.post = post;
        self.IsPostEdit(true);
        self.LoadPostForm();
    }
    self.EditCategory = function (category) {
        self.category = category;
        self.IsCategoryEdit(true);
        self.LoadCategoryForm();
    }
    self.EditTag = function (tag) {
        self.tag = tag;
        self.IsTagEdit(true);
        self.LoadTagForm();
    }

    self.DeletePost = function (post) {


        $.ajax({
            url: '/Admin/DeletePost',
            cache: false,
            type: 'GET',
            data: { postId: post.Id },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {

                self.totalPosts.removeAll();
                self.LoadPosts();
            },
            complete: function () {

            }
        });
    }
    self.DeleteCategory = function (category) {
        $.ajax({
            url: '/Admin/DeleteCategory',
            cache: false,
            type: 'GET',
            data: { categoryId: category.Id },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {

                self.totalCategories("");
                self.LoadCategories();

            },
            complete: function () {

            }
        });
    }
    self.DeleteTag = function (tag) {

        $.ajax({
            url: '/Admin/DeleteTag',
            cache: false,
            type: 'GET',
            data: { tag: tag.Id },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {

                self.totalTags("");
                self.LoadTags();

            },
            complete: function () {

            }
        });
    }

    self.LoadPostForm = function () {
        $("#myAddNewBlogModal .modal-body").html("");

        $.ajax({
            url: '/Admin/AddEditPost',
            cache: false,
            type: 'GET',
            data: { postId: self.post.Id },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                self.modelTitle("AddEditPost");

                $('#myAddNewBlogModal').appendTo("body").modal('show');

                $("#myAddNewBlogModal .modal-body").html(data);
                tinyMCE.execCommand('mceAddControl', true, "ShortDescription");
                tinyMCE.execCommand('mceAddControl', true, "Description");
            }
        });
        if (self.IsPostEdit() == true) {
            //    alert(12);
            self.post = "";
            self.IsPostEdit(false);
        }

    };
    self.LoadCategoryForm = function () {
        $("#myAddNewBlogModal .modal-body").html("");
        $.ajax({
            url: '/Admin/AddEditCategory',
            cache: false,
            type: 'GET',
            data: { categoryId: self.category.Id },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                self.modelTitle("AddEditCategory");
                $('#myAddNewBlogModal').appendTo("body").modal('show');

                $("#myAddNewBlogModal .modal-body").html(data);

            },
            complete: function () {

            }
        });
        if (self.IsCategoryEdit() == true) {
            //    alert(12);
            self.category = "";
            self.IsCategoryEdit(false);
        }
    };
    self.LoadTagForm = function () {
        $("#myAddNewBlogModal .modal-body").html("");
        $.ajax({
            url: '/Admin/AddEditTag',
            cache: false,
            type: 'GET',
            data: { tagId: self.tag.Id },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                self.modelTitle("AddEditTag");
                $('#myAddNewBlogModal').appendTo("body").modal('show');

                $("#myAddNewBlogModal .modal-body").html(data);

            },
            complete: function () {


            }
        });
        if (self.IsTagEdit() == true) {
            //    alert(12);
            self.tag = "";
            self.IsTagEdit(false);
        }
    };

    self.savePostCategoryTag = function () {

        var form = $("#myAddNewBlogModal .modal-body").find("form");
        // console.log(form);

        if (form.attr("id") == "AddEditPostForm") {

            $("#" + form.attr("id") + "    :text,textarea").each(function (index, element) {

                var id = $(this).attr("id"); // returns the object ID

                if (id == "ShortDescription") {

                    $(this).val(tinyMCE.get("ShortDescription").getContent());
                } else if (id == "Description") {

                    $(this).val(tinyMCE.get("Description").getContent());
                }
            });
            // etc
        }
        //  if ("#"+$(form.attr("id")).valid()) {
        var action = form.attr("action");
        // alert(action);
        $.ajax({
            cache: false,
            url: action,
            data: form.serialize(),

            success: function (res) {
                if (form.attr("id") == "AddEditPostForm") {
                    //       self.totalPosts("");
                    self.LoadPosts();
                } else if (form.attr("id") == "AddEditCategoryForm") {
                    //    self.totalCategories("");
                    self.LoadCategories();
                } else if (form.attr("id") == "AddEditTagForm") {
                    //     self.totalTags("");
                    self.LoadTags();
                }
                $('#myAddNewBlogModal').modal('hide');
            },
            type: "POST",
            error: function (a, b, c) {
                alert(a);
                alert(b);
                alert(c);
            }
        });
        // }
    };

    self.LoadPosts = function () {

        $.ajax({
            url: '/Admin/GetPosts',
            cache: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                
                self.totalPosts(data.rows);
                $('#userPost').dataTable();

            },
            complete: function () {


            }
        });

    };
    self.LoadCategories = function () {

        if (self.totalCategories() == "") {
            $.ajax({
                url: '/Admin/GetCategories',
                cache: false,
                type: 'GET',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    self.totalCategories(data.rows);
                    $('#userCategory').dataTable();
                },
                complete: function () {


                }
            });
        }
    };
    self.LoadTags = function () {

        if (self.totalTags() == "") {
            $.ajax({
                url: '/Admin/GetTags',
                cache: false,
                type: 'GET',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    self.totalTags(data.rows);

                    $('#userTeg').dataTable();
                },
                complete: function () {


                }
            });
        }
    };




}

var viewModel = new BlogViewModel();
ko.applyBindings(viewModel);