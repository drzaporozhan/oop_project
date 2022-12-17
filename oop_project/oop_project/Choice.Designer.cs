
namespace oop_project
{
    partial class Choice
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CreateTask = new System.Windows.Forms.Button();
            this.ShowTasks = new System.Windows.Forms.Button();
            this.History = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CreateTask
            // 
            this.CreateTask.Font = new System.Drawing.Font("Comic Sans MS", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CreateTask.Location = new System.Drawing.Point(202, 67);
            this.CreateTask.Name = "CreateTask";
            this.CreateTask.Size = new System.Drawing.Size(375, 59);
            this.CreateTask.TabIndex = 0;
            this.CreateTask.Text = "Создать задачу";
            this.CreateTask.UseVisualStyleBackColor = true;
            this.CreateTask.Click += new System.EventHandler(this.button1_Click);
            // 
            // ShowTasks
            // 
            this.ShowTasks.Font = new System.Drawing.Font("Comic Sans MS", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ShowTasks.Location = new System.Drawing.Point(202, 178);
            this.ShowTasks.Name = "ShowTasks";
            this.ShowTasks.Size = new System.Drawing.Size(375, 59);
            this.ShowTasks.TabIndex = 1;
            this.ShowTasks.Text = "Посмотреть список задач";
            this.ShowTasks.UseVisualStyleBackColor = true;
            this.ShowTasks.Click += new System.EventHandler(this.button2_Click);
            // 
            // History
            // 
            this.History.Font = new System.Drawing.Font("Comic Sans MS", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.History.Location = new System.Drawing.Point(202, 289);
            this.History.Name = "History";
            this.History.Size = new System.Drawing.Size(375, 59);
            this.History.TabIndex = 2;
            this.History.Text = "История просмотра";
            this.History.UseVisualStyleBackColor = true;
            this.History.Click += new System.EventHandler(this.button3_Click);
            // 
            // Choice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.History);
            this.Controls.Add(this.ShowTasks);
            this.Controls.Add(this.CreateTask);
            this.Name = "Choice";
            this.Text = "Choice";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CreateTask;
        private System.Windows.Forms.Button ShowTasks;
        private System.Windows.Forms.Button History;
    }
}