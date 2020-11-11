import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PostQueryBody, Repository, QueryBrowserResult } from "../models/repository";
import { DateObservableService } from "../websockets/dateObservable.service";
import { TextObservableService } from "../websockets/textObservable.service";
import { Observable } from 'rxjs';
import { AbstractControl, FormControl, FormGroup } from '@angular/forms';

@Component({
	selector: 'app-develop',
	templateUrl: './develop.component.html',
	styleUrls: ['./develop.component.css']
})
export class DevelopComponent implements OnInit
{
	title = 'Hello from DevelopComponent.';
	public dateObservable: Observable<Date>;
	public textObservable: Observable<string>;

	browserTextArea0: string;
	browserTextArea1: string;
	browserTextArea2: string;
	browserTextArea3: string;
	isError: boolean;
	isSuccess: boolean;

	myFormModel: FormGroup;

	constructor(private repository: Repository,
		private httpClient: HttpClient,
		private dateObservableService: DateObservableService,
		public textObservableService: TextObservableService)
	{
		this.dateObservable = dateObservableService.createObservableService();
		this.textObservable = textObservableService.createObservableService();

		this.myFormModel = new FormGroup(
			{
				browseUrl: new FormControl('https://amazon.com/'),
				subjectText: new FormControl(3.14159)
			}
		)
	}

	ngOnInit(): void
	{
		this.myFormModel.patchValue(
			{
				subjectText: 'C#'
			})
	}

	clearTextArea()
	{
		this.browserTextArea0 = "";
		this.browserTextArea1 = "";
		this.browserTextArea2 = "";
		this.browserTextArea3 = "";
	}

	//	Handle the submit button.
	onSubmit()
	{
		this.clearTextArea();

		console.log("onSubmit");
		let browseUrl: string = this.myFormModel.controls['browseUrl'].value;
		let postQueryBody = new PostQueryBody(browseUrl, "C++");
		let queryBrowserResult = this.repository.queryBrowserPost(postQueryBody);
		queryBrowserResult.subscribe(data =>
		{
			this.browserTextArea0 = data.textArray[0] ?? "unknown";
			this.browserTextArea1 = data.textArray[1] ?? "unknown";
			this.browserTextArea2 = data.textArray[2] ?? "unknown";
			this.browserTextArea3 = data.textArray[3] ?? "unknown";
			this.isError = data.isError;
			this.isSuccess = data.isSuccess;
		});
		console.log(this.myFormModel.value);
		console.log("\n");
	}
}
