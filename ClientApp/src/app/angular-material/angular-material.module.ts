import { NgModule } from '@angular/core';

// Only those Angular Material Modules needed should be imported here.
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSliderModule } from '@angular/material/slider';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
//	Declare the providers for animation services.
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

const MaterialModules = [
	MatAutocompleteModule,
	MatBadgeModule,
	MatButtonModule,
	MatButtonToggleModule,
	MatCardModule,
	MatCheckboxModule,
	MatDatepickerModule,
	MatDialogModule,
	MatDividerModule,
	MatExpansionModule,
	MatFormFieldModule,
	MatIconModule,
	MatGridListModule,
	MatInputModule,
	MatListModule,
	MatMenuModule,
	MatProgressSpinnerModule,
	MatRadioModule,
	MatSelectModule,
	MatSidenavModule,
	MatSliderModule,
	MatSnackBarModule,
	MatStepperModule,
	MatTableModule,
	MatTabsModule,
	MatToolbarModule,
	MatTooltipModule,
	BrowserAnimationsModule
];

@NgModule({
	imports: [
		MaterialModules
	],
	//	Reexport the AM modules so they can be used in other modules.
	exports: [
		MaterialModules
	]
})
export class AngularMaterialModule { }


//	The icons associated with thier keys.
//	https://www.angularjswiki.com/angular/angular-material-icons-list-mat-icon-list/